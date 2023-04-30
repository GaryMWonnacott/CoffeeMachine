
namespace CoffeeMachine.Services.DTOs
{
    public class ActionTypeDTO
    {
        public ActionTypeDTO(String name, String message, String failedMessage, int actionTypeId) { 
            Name = name;
            Message = message;
            FailedMessage = failedMessage;
            ActionTypeId = actionTypeId;
        }
        public String Name { get; set; }
        public String Message { get; set; }
        public String FailedMessage { get; set; }
        public int ActionTypeId { get; set; }
    }
}
