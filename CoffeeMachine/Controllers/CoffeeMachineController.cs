using CoffeeMachine.Models;
using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.Application;

namespace CoffeeMachine.Controllers
{
    public class CoffeeMachineController : Controller
    {
        private readonly ICoffeeMachineApplication _CoffeeMachineApplication;
        public CoffeeMachineController(ICoffeeMachineApplication coffeeMachineApplication)
        {
            _CoffeeMachineApplication = coffeeMachineApplication;
        }
        [HttpGet]
        public IActionResult HomeScreen()
        {
            var vm = new HomeScreenVM(_CoffeeMachineApplication);

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> TurnOn()
        {
            var application = await _CoffeeMachineApplication.TurnOn();
            var vm = new HomeScreenVM(application);

            return PartialView("HomeScreen", vm);
        }

        [HttpGet]
        public async Task<IActionResult> TurnOff()
        {
            var application = await _CoffeeMachineApplication.TurnOff();
            var vm = new HomeScreenVM(application);

            return PartialView("HomeScreen", vm);
        }
        [HttpGet]
        public async Task<IActionResult> MakeCoffee(int numEspressoShots, bool addMilk)
        {
            var application = await _CoffeeMachineApplication.MakeCoffee(numEspressoShots, addMilk);
            var vm = new HomeScreenVM(application, numEspressoShots, addMilk);

            return PartialView("HomeScreen", vm);
        }

        public async Task<IActionResult> UtilisationReport()
        {
            var vm = new UtilisationVM(await _CoffeeMachineApplication.DailySummaryGet(), await _CoffeeMachineApplication.HourlySummaryGet());

            return View(vm);
        }
    }
}
