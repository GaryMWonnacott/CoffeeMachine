using CoffeeMachine.Services.DTOs;

namespace CoffeeMachine.Services.DataAccess
{
    public interface IDataAccess
    {
        public Task ActionLog(CoffeeMachineActionDTO action);
        public Task<IList<HourlySummaryRowDTO>> HourlySummaryGet();
        public Task<IList<DailySummaryRowDTO>> DailySummaryGet();
    }

}
