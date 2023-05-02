namespace CoffeeMachine.Services.DTOs
{
    public class DailySummaryRowDTO
    {
        public DailySummaryRowDTO(String dayOfWeek, String minTime, String maxTime, int averageCoffees)
        {
            DayOfWeek = dayOfWeek;
            MinTime = minTime;
            MaxTime = maxTime;
            AverageCoffees = averageCoffees;
        }
        public String DayOfWeek { get; set; }

        public String? MinTime { get; set; }
        public String? MaxTime { get; set; }
        public int AverageCoffees { get; set; }
    }
}
