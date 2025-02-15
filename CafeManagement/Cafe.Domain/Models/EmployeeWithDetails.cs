using Cafe.Domain.Entities;

namespace Cafe.Domain.Models
{
    public class EmployeeWithDetails
    {
        public EmployeeEntity Employee { get; }
        public CafeEntity AssignedCafe { get; }
        public List<CafeEmployeeEntity> CafeEmployments { get; }

        public EmployeeWithDetails(EmployeeEntity employee, CafeEntity assignedCafe, List<CafeEmployeeEntity> cafeEmployments)
        {
            Employee = employee;
            AssignedCafe = assignedCafe;
            CafeEmployments = cafeEmployments ?? new List<CafeEmployeeEntity>();
        }
    }
}
