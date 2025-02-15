namespace Cafe.Application.DTOs
{
    public class CafeEmployeeDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid CafeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
