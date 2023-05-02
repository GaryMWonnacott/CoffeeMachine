namespace CoffeeMachine.Application
{
    public class CoffeeMachineState
    {
        public CoffeeMachineState(String name)
        {
            Name = name;
            Message = "Machine is " + name;
        }
        public CoffeeMachineState(String name, String message)
        {
            Name = name;
            Message = message;
        }
        public String Name;
        public String Message;
    }

}
