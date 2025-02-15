using Cafe.Domain.Entities;

namespace Cafe.Domain.Models
{
    public class CafeWithEmployees
    {
        public CafeEntity Cafe { get; set; }
        public ICollection<CafeEmployeeEntity> Employees { get; set; }

        public CafeWithEmployees(CafeEntity cafe, ICollection<CafeEmployeeEntity> employees)
        {
            Cafe = cafe;
            Employees = employees ?? new List<CafeEmployeeEntity>();
        }
    }
}
