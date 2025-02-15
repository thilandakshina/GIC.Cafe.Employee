namespace Cafe.Domain.Entities
{
    public class CafeEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
        public virtual ICollection<CafeEmployeeEntity> CafeEmployees { get; set; } = new List<CafeEmployeeEntity>();

        public void Update(CafeEntity updatedCafe)
        {
            Name = updatedCafe.Name;
            Description = updatedCafe.Description;
            Location = updatedCafe.Location;
            Logo = updatedCafe.Logo;
            ModifiedDate = DateTime.UtcNow;
        }
    }
}