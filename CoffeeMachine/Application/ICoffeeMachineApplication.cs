﻿using CoffeeMachine.Services.DTOs;

namespace CoffeeMachine.Application
{
    public interface ICoffeeMachineApplication
    {
        public void StateCurrentSet(CoffeeMachineState state);
        public CoffeeMachineState StateCurrentGet();
        public Task<IDictionary<String, CoffeeMachineActionType>> ActionTypesGet();
        public Task<IList<CoffeeMachineElement>> CoffeeMachineElementsGet();
        public Task<String> LastActionMessageGet();
        public Task<ICoffeeMachineApplication> TurnOn();
        public Task<ICoffeeMachineApplication> TurnOff();
        public Task<ICoffeeMachineApplication> MakeCoffee(int numEspressoShots, bool addMilk);
        public Task<IList<HourlySummaryRowDTO>> HourlySummaryGet();
        public Task<IList<DailySummaryRowDTO>> DailySummaryGet();
    }
}
