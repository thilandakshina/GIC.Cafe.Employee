using Cafe.Domain.Entities;

namespace Cafe.Domain.Models
{
    public class CafeWithEmployees
    {
        public CafeEntity Cafe { get; }
        public ICollection<CafeEmployeeEntity> Employees { get; }

        public CafeWithEmployees(CafeEntity cafe, ICollection<CafeEmployeeEntity> employees)
        {
            Cafe = cafe;
            Employees = employees ?? new List<CafeEmployeeEntity>();
        }
    }
}
