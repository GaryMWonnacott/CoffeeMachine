using CoffeeMachine.Application;

namespace CoffeeMachine.Models
{
    public class HomeScreenVM
    {
        public HomeScreenVM(ICoffeeMachineApplication coffeeMachineApplication)
        {
            _ = HomeScreenVMSetupFromApplication(coffeeMachineApplication);
        }
        public HomeScreenVM(ICoffeeMachineApplication coffeeMachineApplication, int numEspressoShots, bool addMilk)
        {
            _ = HomeScreenVMSetupFromApplication(coffeeMachineApplication);
            NumEspressoShots = numEspressoShots;
            AddMilk = addMilk;
        }
        private async Task HomeScreenVMSetupFromApplication(ICoffeeMachineApplication coffeeMachineApplication)
        {
            State = coffeeMachineApplication.StateCurrentGet();
            Elements = await coffeeMachineApplication.CoffeeMachineElementsGet();
            LastActionMessage = await coffeeMachineApplication.LastActionMessageGet();
        }
        public CoffeeMachineState State { get; set; }
        public IList<CoffeeMachineElement> Elements { get; set; }
        public String? LastActionMessage { get; set; }
        public int NumEspressoShots { get; set; } = 1;
        public bool AddMilk { get; set; } = false;
    }
}
