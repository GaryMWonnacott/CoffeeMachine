using CoffeeMachine.Services.DTOs;

namespace CoffeeMachine.Application
{
    public class CoffeeMachineActionType
    {
        public CoffeeMachineActionType()
        {
            Name = String.Empty;
            Message = String.Empty;
            FailedMessage = String.Empty;
        }
        public CoffeeMachineActionType(String name, String message, String failedMessage, int actionTypeId)
        {
            Name = name;
            Message = message;
            FailedMessage = failedMessage;
            ActionTypeId = actionTypeId;
        }
        public CoffeeMachineActionType(ActionTypeDTO actionType)
        {
            Name = actionType.Name;
            Message = actionType.Message;
            FailedMessage = actionType.FailedMessage;
            ActionTypeId = actionType.ActionTypeId;
        }
        public String Name { get; set; }
        public String Message { get; set; }
        public String FailedMessage { get; set; }
        public int ActionTypeId { get; set; }
    }
    public class CoffeeMachineActionTypes
    {
        IList<CoffeeMachineActionType> ActionTypes;
        public CoffeeMachineActionTypes(IList<ActionTypeDTO> actionTypes)
        {
            ActionTypes = new List<CoffeeMachineActionType>();

            foreach (var actionType in actionTypes)
            {
                ActionTypes.Add(new CoffeeMachineActionType(actionType));
            }
        }

        public CoffeeMachineActionType ActionTypeGet(String actionType)
        {
            CoffeeMachineActionType ret;

            if (ActionTypes.Any(t => t.Name == actionType))
            {
                ret = ActionTypes.First(t => t.Name == actionType);
            }
            else
            {
                ret = new CoffeeMachineActionType();
            }

            return ret; 
        }
    }
}
