namespace Cafe.Domain.Entities
{
    public class EmployeeEntity : BaseEntity
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public GenderType Gender { get; set; }
        public virtual ICollection<CafeEmployeeEntity> CafeEmployees { get; set; } = new List<CafeEmployeeEntity>();

        public void GenerateEmployeeId()
        {
            EmployeeId = $"UI{Guid.NewGuid().ToString().Substring(0, 7).ToUpper()}";
        }

        public void Update(EmployeeEntity updatedEmployee)
        {
            Name = updatedEmployee.Name ;
            EmailAddress = updatedEmployee.EmailAddress ;
            PhoneNumber = updatedEmployee.PhoneNumber ;
            Gender = updatedEmployee.Gender ;   
        }
    }

    public enum GenderType
    {
        Male,
        Female
    }
}
