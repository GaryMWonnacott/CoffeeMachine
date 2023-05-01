using CoffeeMachine.Application.Interfaces;

namespace CoffeeMachine.Application
{
    public class CoffeeMachineElement
    {
        public CoffeeMachineElement(String name, StateInternal state)
        {
            Name = name;
            State = state;
        }

        public String Name { get; private set; }
        public StateInternal State { get; private set; }
        public String NameGet()
        {
            return Name;
        }
        public StateInternal StateGet()
        {
            return State;
        }
    }
    public class CoffeeMachineElements
    {
        public CoffeeMachineElements(IList<CoffeeMachineElement> elements)
        {
            Elements = elements;
            IsAlert = Elements != null && Elements.Any(m => m.State == StateInternal.Alert);
        }

        public IList<CoffeeMachineElement> Elements { get; set; }
        public bool IsAlert { get; private set; }
        public IDictionary<String, String> ElementsGetAsDictionary()
        {
            var ret = new Dictionary<String, String>();

            foreach (var element in Elements)
            {
                ret.Add(element.Name, element.State.ToString());
            }

            return ret;
        }
    }
}
