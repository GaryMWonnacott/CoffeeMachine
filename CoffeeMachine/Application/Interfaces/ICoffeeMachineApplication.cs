using CoffeeMachine.Services.DTOs;

namespace CoffeeMachine.Application.Interfaces
{
    public interface ICoffeeMachineApplication
    {
        public void StateCurrentSet(CoffeeMachineState state);
        public CoffeeMachineState StateCurrentGet();
        public IDictionary<string, bool> ActionTypesValidityGet();
        public IDictionary<String, String> CoffeeMachineElementsGet();
        public string LastActionMessageGet();
        public Task<ICoffeeMachineApplication> TurnOn();
        public Task<ICoffeeMachineApplication> TurnOff();
        public Task<ICoffeeMachineApplication> MakeCoffee(int numEspressoShots, bool addMilk);
        public Task<IList<HourlySummaryRowDTO>> HourlySummaryGet();
        public Task<IList<DailySummaryRowDTO>> DailySummaryGet();
    }
}
