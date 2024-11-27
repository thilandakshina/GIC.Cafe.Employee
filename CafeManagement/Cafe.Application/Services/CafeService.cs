using Cafe.Application.Services;
using Cafe.Domain.Entities;
using Cafe.Domain.Models;
using Cafe.Infrastructure.UnitOfWork;

public class CafeService : ICafeService
{
    private readonly IUnitOfWork _unitOfWork;

    public CafeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateAsync(CafeEntity cafe)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var result = await _unitOfWork.CafeCommand.AddAsync(cafe);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return result.Id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(CafeEntity updatedCafe)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var cafe = await _unitOfWork.CafeQuery.GetByIdAsync(updatedCafe.Id);
            if (cafe == null)
                throw new ArgumentException($"Cafe with ID {updatedCafe.Id} not found");
            
            cafe.Update(updatedCafe);
            
            await _unitOfWork.CafeCommand.UpdateAsync(cafe);
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
            var cafe = await _unitOfWork.CafeQuery.GetByIdAsync(id);
            if (cafe == null)
                throw new ArgumentException($"Cafe with ID {id} not found");

            await _unitOfWork.CafeEmployeeCommand.UnassignListofEmployeesFromCafeAsync(id);

            cafe.Deactivate();
            await _unitOfWork.CafeCommand.UpdateAsync(cafe);
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

    public async Task<CafeWithEmployees> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.CafeQuery.GetCafeByIdAsync(id);
    }

    public async Task<IEnumerable<CafeWithEmployees>> GetAllAsync(string location = null)
    {
        return await _unitOfWork.CafeQuery.GetAllWithEmployeeCountAsync(location);
    }
}
