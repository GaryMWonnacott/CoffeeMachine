using CoffeeMachine.Services.DataAccess;
using CoffeeMachine.Services.DTOs;

namespace CoffeeMachine.Models
{
    public class UtilisationVM
    {
        public UtilisationVM(){}
        public IList<DailySummaryRowDTO> DailySummary { get; set; }
        public IList<HourlySummaryRowDTO> HourlySummary { get; set; }
    }
}