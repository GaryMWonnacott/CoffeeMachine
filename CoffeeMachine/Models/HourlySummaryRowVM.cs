namespace CoffeeMachine.Models
{
    public class HourlySummaryRowVM
    {
        public HourlySummaryRowVM(String hour, int averageCoffees)
        {
            Hour = hour;
            AverageCoffees = averageCoffees;
        }

        public String Hour { get; set; }

        public int AverageCoffees { get; set; }
    }
}