using CoffeeMachine.Services.CoffeeMachine;

namespace CoffeeMachine.Application
{
    public class CoffeeMachineAction
    {
        public CoffeeMachineAction()
        {
            CoffeeMachineActionSetup(new CoffeeMachineActionType(), true);
        }
        public CoffeeMachineAction(CoffeeMachineActionType actionType, bool isSuccess = true)
        {
            CoffeeMachineActionSetup(actionType, isSuccess);
        }
        public CoffeeMachineAction(CoffeeMachineActionType actionType, CoffeeCreationOptions options, bool isSuccess = true)
        {
            CoffeeMachineActionSetup(actionType, isSuccess);
            Options = options;
        }

        private void CoffeeMachineActionSetup(CoffeeMachineActionType actionType, bool isSuccess)
        {
            ActionType = actionType;
            IsSuccess = isSuccess;
        }

        public CoffeeMachineActionType ActionType { get; set; }
        public bool IsSuccess { get; set; }
        public CoffeeCreationOptions Options;
        public String? AdditionalMessage { get; set; }
        public String Message => String.Concat(IsSuccess ? ActionType.Message : ActionType.FailedMessage, String.IsNullOrWhiteSpace(AdditionalMessage) ? "" : ":" + AdditionalMessage);
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public void CoffeeCreationOptionsSet(int numEspressoShots, bool addMilk)
        {
            Options.NumEspressoShots = numEspressoShots;
            Options.AddMilk = addMilk;
        }

        public String CoffeeCreationOptionsGetAsString()
        {
            return String.Concat("NumEsspressoShots:", Options.NumEspressoShots.ToString(), "|AddMilk:", Options.AddMilk);
        }
    }
}
