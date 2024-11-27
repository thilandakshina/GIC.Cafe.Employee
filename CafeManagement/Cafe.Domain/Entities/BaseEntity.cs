namespace Cafe.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual void Deactivate()
        {
            IsActive = false;
            ModifiedDate = DateTime.UtcNow;
        }
    }
}
