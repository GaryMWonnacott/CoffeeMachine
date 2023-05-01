namespace CoffeeMachine.Application
{
    public class CoffeeMachineAction
    {

        public CoffeeMachineAction(CoffeeMachineActionType actionType, IDictionary<String, Object>? optionValues = null)
        {
            ActionType = actionType;
            OptionValues = optionValues;
            IsSuccess = true;
        }

        public CoffeeMachineAction(CoffeeMachineActionType actionType, CoffeeMachineState state, IDictionary<String, Object>? optionValues = null)
        {
            ActionType = actionType;
            State = state;
            OptionValues = optionValues;
            IsSuccess = IsValid();
        }

        public CoffeeMachineActionType ActionType { get; private set; }
        public CoffeeMachineState? State { get; private set; }
        public bool IsSuccess { get; set; }
        public String? AdditionalMessage { get; set; }
        public String Message => String.Concat(IsSuccess ? ActionType.Message : ActionType.FailedMessage, (!IsValid()&&State!=null ? ":" + State.Message : ""), String.IsNullOrWhiteSpace(AdditionalMessage) ? "" : ":" + AdditionalMessage);
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public IDictionary<String,Object>? OptionValues { get; set; }
        public bool IsValid ()
        {
            return State!=null?ActionType.IsValid(State):true;
        }
        public String OptionValuesGetAsString()
        {
            var ret = String.Empty;
            if (OptionValues != null)
            {
                foreach (KeyValuePair<String, Object> optionValue in OptionValues)
                {
                    ret += String.IsNullOrEmpty(ret) ? "" : "|";
                    ret += String.Concat(optionValue.Key, ':', optionValue.Value);
                }
            }
            return ret;
        }
    }
}
