
namespace CoffeeMachine.Services.DTOs
{
    public class CoffeeMachineActionDTO
    {
        public CoffeeMachineActionDTO(int actionTypeId, bool isSuccess, String message, DateTime timeStamp, String? options = null)
        {
            ActionTypeId = actionTypeId;
            IsSuccess = isSuccess;
            Message = message;
            TimeStamp = timeStamp;
            Options = options;
        }
        public int ActionTypeId { get; set; }
        public bool IsSuccess { get; set; }
        public String Message { get; set; }
        public String? Options { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
