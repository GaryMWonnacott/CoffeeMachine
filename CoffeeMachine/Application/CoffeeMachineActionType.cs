using CoffeeMachine.Models;
using CoffeeMachine.Services.CoffeeMachine;
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
            ValidStates = new List<CoffeeMachineState>();
        }
        public CoffeeMachineActionType(String name, String message, String failedMessage, int actionTypeId, IList<CoffeeMachineState> validStates, IDictionary<String,CoffeeMachineOption>? options = null)
        {
            Name = name;
            Message = message;
            FailedMessage = failedMessage;
            ActionTypeId = actionTypeId;
            ValidStates = validStates;
            Options = options;
        }
        public String Name { get; set; }
        public String Message { get; set; }
        public String FailedMessage { get; set; }
        public int ActionTypeId { get; set; }
        public IList<CoffeeMachineState> ValidStates { get; set; }
        public IDictionary<String, CoffeeMachineOption>? Options { get; set; }
        public bool IsValid(CoffeeMachineState state)
        {
            return ValidStates.Any(m=>m.Name==state.Name);
        }
    }
}
