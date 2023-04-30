using CoffeeMachine.Models;
using CoffeeMachine.Services.DTOs;
using CoffeeMachine.Services.DataAccess;
using CoffeeMachine.Services.CoffeeMachine;
using Microsoft.Extensions.Caching.Memory;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CoffeeMachine.Application
{
    public class CoffeeMachineApplication : ICoffeeMachineApplication
    {
        private readonly IDataAccess _DataAccessService;
        private readonly ICoffeeMachine _CoffeeMachine;
        private readonly IMemoryCache _cache;
        public CoffeeMachineApplication(IDataAccess dataAccessService, ICoffeeMachine coffeeMachineService, IMemoryCache cache)
        {
            _cache = cache;
            _DataAccessService = dataAccessService;
            _CoffeeMachine = coffeeMachineService;

            var actionTypes = _DataAccessService.ActionTypesGet();
            CoffeeMachineActionTypes = new CoffeeMachineActionTypes(actionTypes);

            StatusCurrentSetFromCoffeeMachine();
            LastAction = new CoffeeMachineAction(CoffeeMachineActionTypes.ActionTypeGet("None"));
        }
        private CoffeeMachineState CurrentState { get; set; }
        private IList<CoffeeMachineElement> CoffeeMachineElements { get; set; }
        private CoffeeMachineActionTypes CoffeeMachineActionTypes { get; set; }
        private CoffeeMachineAction LastAction { get; set; }
        public void StateCurrentSet(CoffeeMachineState state)
        {
            CurrentState = state;
        }
        public CoffeeMachineState StateCurrentGet()
        {
            return CurrentState;
        }
        public async Task<IList<CoffeeMachineElement>> CoffeeMachineElementsGet()
        {
            return CoffeeMachineElements;
        }
        public async Task<String> LastActionMessageGet()
        {
            return LastAction.Message;
        }

        private void StatusCurrentSetFromCoffeeMachine()
        {
            CoffeeMachineElements = new List<CoffeeMachineElement>
            {
                new CoffeeMachineElement("Water Level State", (StateInternal)_CoffeeMachine.WaterLevelState),
                new CoffeeMachineElement("Bean Feed State", (StateInternal)_CoffeeMachine.BeanFeedState),
                new CoffeeMachineElement("Waste Coffee State", (StateInternal)_CoffeeMachine.WasteCoffeeState),
                new CoffeeMachineElement("Water Tray State", (StateInternal)_CoffeeMachine.WaterTrayState)
            };

            if (!_CoffeeMachine.IsOn)
            {
                CurrentState = new Off();
            }
            else if (_CoffeeMachine.IsMakingCoffee)
            {
                CurrentState = new Active();
            }
            else if (CoffeeMachineElements.Any(m => m.State == StateInternal.Alert))
            {
                CurrentState = new Alert();
            }
            else
            {
                CurrentState = new Idle();
            }
        }

        public async Task<ICoffeeMachineApplication> TurnOn()
        {
            var action = new CoffeeMachineAction(CoffeeMachineActionTypes.ActionTypeGet("TurnOn"));

            StatusCurrentSetFromCoffeeMachine();

            if (CurrentState.CanTurnOn(action))
            {
                try
                {
                    CurrentState = new Idle();
                    await _CoffeeMachine.TurnOnAsync();
                    StatusCurrentSetFromCoffeeMachine();
                }
                catch
                {
                    action.IsSuccess = false;
                }
            }
            else
            {
                action.IsSuccess = false;
            }

            await ActionProcess(action);

            return this;
        }

        public async Task<ICoffeeMachineApplication> TurnOff()
        {
            var action = new CoffeeMachineAction(CoffeeMachineActionTypes.ActionTypeGet("TurnOff"));
            StatusCurrentSetFromCoffeeMachine();

            if (CurrentState.CanTurnOff(action))
            {
                try
                {
                    CurrentState = new Off();
                    await _CoffeeMachine.TurnOffAsync();
                }
                catch
                {
                    action.IsSuccess = false;
                }
            }
            else
            {
                action.IsSuccess = false;
            }

            await ActionProcess(action);

            return this;
        }

        public async Task<ICoffeeMachineApplication> MakeCoffee(int numEspressoShots, bool addMilk)
        {
            var options = new CoffeeCreationOptions(numEspressoShots, addMilk);
            var action = new CoffeeMachineAction(CoffeeMachineActionTypes.ActionTypeGet("MakeCoffee"), options);
            StatusCurrentSetFromCoffeeMachine();

            if (CurrentState.CanMakeCoffee(action))
            {
                try
                {
                    CurrentState = new Active();
                    await _CoffeeMachine.MakeCoffeeAsync(options);
                    CurrentState = new Idle();
                }
            catch
            {
                action.IsSuccess = false;
                }
            }
            else
            {
                action.IsSuccess = false;
            }

            await ActionProcess(action);

            return this;
        }

        public async Task<IList<DailySummaryRowDTO>> DailySummaryGet()
        {
            var ret = await _DataAccessService.DailySummaryGet();

            return ret;

        }

        public async Task<IList<HourlySummaryRowDTO>> HourlySummaryGet()
        {
            var ret = await _DataAccessService.HourlySummaryGet();
            return ret;
        }

        private async Task ActionProcess(CoffeeMachineAction action)
        {
            LastAction = action;
            var coffeeMachineActionDTO = new CoffeeMachineActionDTO(action.ActionType.ActionTypeId, action.IsSuccess, action.Message, action.TimeStamp, action.CoffeeCreationOptionsGetAsString());
            await _DataAccessService.ActionLog(coffeeMachineActionDTO);
        }
    }
}
