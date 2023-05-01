using CoffeeMachine.Services.DTOs;
using CoffeeMachine.Services.DataAccess;
using CoffeeMachine.Services.CoffeeMachine;
using Microsoft.Extensions.Caching.Memory;
using CoffeeMachine.Application.Interfaces;

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

            StatusCurrentSetFromCoffeeMachine();

            LastAction = new CoffeeMachineAction(ActionTypeList["None"]);
        }
        private CoffeeMachineState CurrentState { get; set; }
        private CoffeeMachineElements CoffeeMachineElements { get; set; }
        private CoffeeMachineAction LastAction { get; set; }
        public void StateCurrentSet(CoffeeMachineState state)
        {
            CurrentState = state;
        }
        public CoffeeMachineState StateCurrentGet()
        {
            return CurrentState;
        }
        public IDictionary<String, bool> ActionTypesValidityGet()
        {
            var ret = new Dictionary<String, bool>();

            foreach(KeyValuePair<String, CoffeeMachineActionType> actionType in ActionTypeList)
            {
                ret.Add(actionType.Key, actionType.Value.IsValid(CurrentState));
            }
            return ret;
        }
        public IDictionary<String,String> CoffeeMachineElementsGet()
        {
            return CoffeeMachineElements.ElementsGetAsDictionary();
        }
        public String LastActionMessageGet()
        {
            return LastAction.Message;
        }

        private void StatusCurrentSetFromCoffeeMachine()
        {
            CoffeeMachineElements = new CoffeeMachineElements(
                new List<CoffeeMachineElement>
                    {
                        new CoffeeMachineElement("Water Level State", (StateInternal)_CoffeeMachine.WaterLevelState),
                        new CoffeeMachineElement("Bean Feed State", (StateInternal)_CoffeeMachine.BeanFeedState),
                        new CoffeeMachineElement("Waste Coffee State", (StateInternal)_CoffeeMachine.WasteCoffeeState),
                        new CoffeeMachineElement("Water Tray State", (StateInternal)_CoffeeMachine.WaterTrayState)
                    }
            );

            CurrentState = StateConverter.StateGet(_CoffeeMachine.IsOn, _CoffeeMachine.IsMakingCoffee, CoffeeMachineElements.IsAlert);
        }

        public async Task<ICoffeeMachineApplication> TurnOn()
        {
            //StatusCurrentSetFromCoffeeMachine();
            var action = new CoffeeMachineAction(ActionTypeList["TurnOn"], CurrentState);

            if (action.IsValid())
            {
                try
                {
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
            //StatusCurrentSetFromCoffeeMachine();
            var action = new CoffeeMachineAction(ActionTypeList["TurnOff"], CurrentState);

            if (action.IsValid())
            {
                try
                {
                    await _CoffeeMachine.TurnOffAsync();
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

        public async Task<ICoffeeMachineApplication> MakeCoffee(int numEspressoShots, bool addMilk)
        {
            var actionOptions = new Dictionary<string, object>()
            {
                ["NumEspressoShots"] = numEspressoShots,
                ["AddMilk"] = addMilk
            };

            //StatusCurrentSetFromCoffeeMachine();
            var action = new CoffeeMachineAction(ActionTypeList["MakeCoffee"], CurrentState, optionValues: actionOptions);

            if (action.IsValid())
            {
                try
                {
                    CurrentState = StateList["Active"];
                    await _CoffeeMachine.MakeCoffeeAsync(CoffeeCreationOptionsFromOptionValues(actionOptions));
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

        private CoffeeCreationOptions CoffeeCreationOptionsFromOptionValues(IDictionary<String, Object> optionValues)
        {
            return new CoffeeCreationOptions((int)optionValues["NumEspressoShots"], (bool)optionValues["AddMilk"]);
        }

        private async Task ActionProcess(CoffeeMachineAction action)
        {
            LastAction = action;
            var coffeeMachineActionDTO = new CoffeeMachineActionDTO(action.ActionType.ActionTypeId, action.IsSuccess, action.Message, action.TimeStamp, action.OptionValuesGetAsString());
            await _DataAccessService.ActionLog(coffeeMachineActionDTO);
        }

        private static readonly IDictionary<String, CoffeeMachineActionType> ActionTypeList =
            new Dictionary<String, CoffeeMachineActionType>()
            {
                ["None"] = new CoffeeMachineActionType(),
                ["TurnOff"] = new CoffeeMachineActionType(
                    "TurnOff",
                    "Turned off",
                    "Failed to turn off",
                    1,
                    new List<CoffeeMachineState>()
                    {
                        new CoffeeMachineState("Idle"),
                        new CoffeeMachineState("Alert")
                    }
                    ),
                ["MakeCoffee"] = new CoffeeMachineActionType(
                    "MakeCoffee", 
                    "Made coffee", 
                    "Failed to make coffee", 
                    2,
                    new List<CoffeeMachineState>()
                    {
                        new CoffeeMachineState("Idle")
                    },
                    new Dictionary<String,CoffeeMachineOption>()
                    {
                        ["NumEspressoShots"]=new CoffeeMachineOption("NumEspressoShots","int"),
                        ["AddMilk"] = new CoffeeMachineOption("NumEspressoShots", "bool")
                    }
                    ),
                ["TurnOn"] = new CoffeeMachineActionType(
                    "TurnOn", 
                    "Turned on", 
                    "Failed to turn on", 
                    3,
                    new List<CoffeeMachineState>()
                    {
                        new CoffeeMachineState("Off")
                    }
                    )
            };

        private static readonly IDictionary<String, CoffeeMachineState> StateList =
            new Dictionary<String, CoffeeMachineState>()
            {
                ["Active"] = new CoffeeMachineState("Active"),
                ["Idle"] = new CoffeeMachineState("Idle"),
                ["Off"] = new CoffeeMachineState("Off"),
                ["Alert"] = new CoffeeMachineState("Alert")
            };

        private static readonly CoffeeMachineStateConverter StateConverter =
            new CoffeeMachineStateConverter(
                new List<CoffeeMachineStateConverterRule>()
                {
                    new CoffeeMachineStateConverterRule(order:1,isOn:false,state:StateList["Off"]),
                    new CoffeeMachineStateConverterRule(order:2,isMakingCoffee:true,state:StateList["Active"]),
                    new CoffeeMachineStateConverterRule(order:3,isAlert:true,state:StateList["Alert"]),
                    new CoffeeMachineStateConverterRule(order:4,state:StateList["Idle"])
                }
            );
    }
}
