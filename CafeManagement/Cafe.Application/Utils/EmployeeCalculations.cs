using Cafe.Domain.Entities;

namespace Cafe.Application.Utils
{
    public static class EmployeeCalculations
    {
        public static int CalculateTotalDaysWorked(IEnumerable<CafeEmployeeEntity> employments)
        {
            var totalDays = 0;
            foreach (var employment in employments)
            {
                var endDate = employment.EndDate ?? (employment.IsActive ? DateTime.Today : employment.EndDate);
                if (endDate.HasValue)
                {
                    totalDays += (endDate.Value - employment.StartDate).Days + 1;
                }
            }
            return totalDays;
        }
    }
}
