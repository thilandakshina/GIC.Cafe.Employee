using System.Xml.Linq;

namespace Cafe.Domain.Entities
{
    public class CafeEmployeeEntity : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public Guid CafeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual EmployeeEntity EmployeeEntity { get; set; }
        public virtual CafeEntity CafeEntity { get; set; }

        public void Add(Guid employeeId, Guid cafeId)
        {
            EmployeeId = employeeId;
            CafeId = cafeId;
            IsActive = true;
            StartDate = DateTime.UtcNow;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            EndDate = DateTime.UtcNow;
        }
    }


}
