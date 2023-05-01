using CoffeeMachine.Application;

namespace CoffeeMachine.Application
{
    public class CoffeeMachineActionTypeValid
    {
        public CoffeeMachineState State { get; private set; }
        public CoffeeMachineActionType ActionType { get; private set; }
        public CoffeeMachineActionTypeValid(CoffeeMachineState state, CoffeeMachineActionType actionType)
        {
            State = state;
            ActionType = actionType;
        }
    }
}