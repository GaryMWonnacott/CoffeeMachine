using CoffeeMachine.Application.Interfaces;

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

            Elements = coffeeMachineApplication.CoffeeMachineElementsGet();

            LastActionMessage = coffeeMachineApplication.LastActionMessageGet();

            ActionTypes = coffeeMachineApplication.ActionTypesValidityGet();
        }
        public String State { get; set; }
        public IDictionary<String,bool> ActionTypes { get; set; }
        public IDictionary<String,String> Elements { get; set; }
        public String? LastActionMessage { get; set; }
        public int NumEspressoShots { get; set; } = 1;
        public bool AddMilk { get; set; } = false;
    }
}
