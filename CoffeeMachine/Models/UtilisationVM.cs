using CoffeeMachine.Services.DataAccess;
using CoffeeMachine.Services.DTOs;

namespace CoffeeMachine.Models
{
    public class UtilisationVM
    {
        public UtilisationVM(IList<DailySummaryRowDTO> dailySummary, IList<HourlySummaryRowDTO> hourlySummary) 
        {
            DailySummary = dailySummary;
            HourlySummary = hourlySummary;
        }
        public IList<DailySummaryRowDTO> DailySummary { get; set; }
        public IList<HourlySummaryRowDTO> HourlySummary { get; set; }
    }
}