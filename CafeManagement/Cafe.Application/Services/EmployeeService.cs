using Cafe.Domain.Entities;
using Cafe.Domain.Models;
using Cafe.Infrastructure.UnitOfWork;

namespace Cafe.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateAsync(EmployeeEntity employee, Guid? cafeId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (cafeId.HasValue)
                {
                    var isExists = await _unitOfWork.CafeCommand.ExistsAsync(cafeId.Value);
                    if (!isExists)
                        throw new ArgumentException($"Cafe with ID {cafeId} not found");
                }

                employee.GenerateEmployeeId();
                var createdEmployee = await _unitOfWork.EmployeeCommand.AddAsync(employee);

                if (cafeId.HasValue)
                {
                    var cafeEmployee = new CafeEmployeeEntity();
                    cafeEmployee.Add(createdEmployee.Id, cafeId.Value);
                    await _unitOfWork.CafeEmployeeCommand.AddAsync(cafeEmployee);
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return createdEmployee.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateAsync(EmployeeEntity updatedEmployee, Guid? cafeId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var employee = await _unitOfWork.EmployeeQuery.GetByIdAsync(updatedEmployee.Id);
                if (employee == null)
                    throw new ArgumentException($"Employee with ID {updatedEmployee.Id} not found");

                if (cafeId.HasValue)
                {
                    var isCafeExists = await _unitOfWork.CafeCommand.ExistsAsync(cafeId.Value);
                    if (!isCafeExists)
                        throw new ArgumentException($"Cafe with ID {cafeId} not found");
                }

                var cafeEmployee = await _unitOfWork.CafeEmployeeQuery.GetCurrentEmploymentAsync(updatedEmployee.Id);
                if (cafeEmployee != null && !cafeId.HasValue)
                {
                    await _unitOfWork.CafeEmployeeCommand.UnassignEmployeeFromCafeAsync(updatedEmployee.Id);
                }
                else if (cafeId.HasValue)
                {
                    await _unitOfWork.CafeEmployeeCommand.AssignEmployeeToCafeAsync(updatedEmployee.Id, cafeId.Value);
                }

                employee.Update(updatedEmployee);

                await _unitOfWork.EmployeeCommand.UpdateAsync(employee);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var employee = await _unitOfWork.EmployeeQuery.GetByIdAsync(id);
                if (employee == null)
                    throw new ArgumentException($"Employee with ID {id} not found");

                await _unitOfWork.CafeEmployeeCommand.UnassignEmployeeFromCafeAsync(id);

                employee.Deactivate();

                await _unitOfWork.EmployeeCommand.UpdateAsync(employee);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<EmployeeWithDetails> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.EmployeeQuery.GetEmployeeWithDetailsAsync(id);
        }

        public async Task<IEnumerable<EmployeeWithDetails>> GetAllAsync(Guid? cafeId)
        {
            var employeesWithDetails = await _unitOfWork.EmployeeQuery.GetAllEmployeesWithDetailsAsync();
            if (cafeId.HasValue)
            {
                employeesWithDetails = employeesWithDetails
                    .Where(e => e.AssignedCafe?.Id == cafeId.Value)
                    .ToList();
            }
            return employeesWithDetails;
        }
    }
}
