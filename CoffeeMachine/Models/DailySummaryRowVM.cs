namespace CoffeeMachine.Models
{
    public class DailySummaryRowVM
    {
        public DailySummaryRowVM(String dayOfWeek, String minTime, String maxTime)
        {
            DayOfWeek = dayOfWeek;
            MinTime = minTime;
            MaxTime = maxTime;
        }
        public String DayOfWeek { get; set; }

        public String? MinTime { get; set; }
        public String? MaxTime { get; set; }
    }
}