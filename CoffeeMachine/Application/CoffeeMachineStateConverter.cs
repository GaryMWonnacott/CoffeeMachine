namespace CoffeeMachine.Application
{
    public class CoffeeMachineStateConverter
    {
        public CoffeeMachineStateConverter(IList<CoffeeMachineStateConverterRule> rules)
        {
            Rules = rules;
        }
        public IList<CoffeeMachineStateConverterRule> Rules { get; set; }
        public CoffeeMachineState StateGet(bool isOn, bool isMakingCoffee, bool isAlert)
        {
            return Rules.OrderBy(o=>o.Order).First(m => 
                (m.IsOn == isOn || !m.IsOn.HasValue)
                &&(m.IsMakingCoffee == isMakingCoffee || !m.IsMakingCoffee.HasValue)
                &&(m.IsAlert == isAlert || !m.IsAlert.HasValue)
            ).State;
        }
    }
    public class CoffeeMachineStateConverterRule
    {
        public CoffeeMachineStateConverterRule(CoffeeMachineState state, int order, bool? isOn = null, bool? isMakingCoffee = null, bool? isAlert = null)
        {
            IsOn = isOn;
            IsMakingCoffee = isMakingCoffee;
            IsAlert = isAlert;
            State = state;
            Order = order;
        }
        public bool? IsOn { get; set; }
        public bool? IsMakingCoffee { get; set; }
        public bool? IsAlert { get; set; }
        public CoffeeMachineState State { get; set; }
        public int Order { get; set; }
    }

}
