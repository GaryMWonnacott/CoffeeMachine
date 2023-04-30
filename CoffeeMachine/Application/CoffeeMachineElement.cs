using CoffeeMachine.Services.CoffeeMachine;

namespace CoffeeMachine.Application
{
    public class CoffeeMachineElement
    {
        public CoffeeMachineElement(string name, StateInternal state)
        {
            Name = name;
            State = state;
        }

        public string Name { get; set; }
        public StateInternal State { get; set; }
    }
    public class CoffeeMachineElements
    {
        public CoffeeMachineElements(string name, StateInternal state)
        {
            Name = name;
            State = state;
        }

        public string Name { get; set; }
        public StateInternal State { get; set; }
    }
}
