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
        public CoffeeMachineElements(IList<CoffeeMachineElement> elements)
        {
            Elements = elements;
            IsAlert = Elements != null && Elements.Any(m => m.State == StateInternal.Alert);
        }

        public IList<CoffeeMachineElement>? Elements { get; set; }
        public bool IsAlert { get; private set; }
    }
}
