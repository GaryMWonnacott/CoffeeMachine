using CoffeeMachine.Application;

namespace CoffeeMachine.Models
{
    public class HomeScreenVM
    {
        public HomeScreenVM(ICoffeeMachineApplication coffeeMachineApplication)
        {
            HomeScreenVMSetupFromApplication(coffeeMachineApplication);
        }
        public HomeScreenVM(ICoffeeMachineApplication coffeeMachineApplication, int numEspressoShots, bool addMilk)
        {
            HomeScreenVMSetupFromApplication(coffeeMachineApplication);
            NumEspressoShots = numEspressoShots;
            AddMilk = addMilk;
        }
        private void HomeScreenVMSetupFromApplication(ICoffeeMachineApplication coffeeMachineApplication)
        {
            var state = coffeeMachineApplication.StateCurrentGet();

            State = state.Name;

            Elements = new Dictionary<String, String>();

            foreach(CoffeeMachineElement element in coffeeMachineApplication.CoffeeMachineElementsGet())
            {
                Elements.Add(element.Name, element.State.ToString());
            };

            LastActionMessage = coffeeMachineApplication.LastActionMessageGet();

            ActionTypes = new Dictionary<String, bool>();

            foreach (KeyValuePair<String,CoffeeMachineActionType> actionType in coffeeMachineApplication.ActionTypesGet())
            {
                var valid = actionType.Value.IsValid(state);
                ActionTypes.Add(actionType.Key, actionType.Value.IsValid(state));

            };
        }
        public String State { get; set; }
        public IDictionary<String,bool> ActionTypes { get; set; }
        public IDictionary<String,String> Elements { get; set; }
        public String? LastActionMessage { get; set; }
        public int NumEspressoShots { get; set; } = 1;
        public bool AddMilk { get; set; } = false;
    }
}
