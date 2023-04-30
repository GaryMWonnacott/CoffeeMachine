
namespace CoffeeMachine.Services.DTOs
{
    public class HourlySummaryRowDTO
    {
        public HourlySummaryRowDTO(String hour, int averageCoffees)
        {
            Hour = hour;
            AverageCoffees = averageCoffees;
        }

        public String Hour { get; set; }

        public int AverageCoffees { get; set; }
    }
}
