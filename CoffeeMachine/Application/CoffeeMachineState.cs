namespace CoffeeMachine.Application
{
    public interface CoffeeMachineState
    {
        public string DescriptionGet();
        public bool CanTurnOn();
        public bool CanTurnOn(CoffeeMachineAction action);
        public bool CanTurnOff();
        public bool CanTurnOff(CoffeeMachineAction action);
        public bool CanMakeCoffee();
        public bool CanMakeCoffee(CoffeeMachineAction action);
    }

    class Off : CoffeeMachineState
    {
        private string Reason = "Machine is active";
        public string DescriptionGet()
        {
            return "Off";
        }
        public bool CanTurnOff()
        {
            return false;
        }
        public bool CanTurnOff(CoffeeMachineAction action)
        {
            action.AdditionalMessage = Reason;
            return CanTurnOff();
        }
        public bool CanTurnOn()
        {
            return true;
        }
        public bool CanTurnOn(CoffeeMachineAction action)
        {
            return CanTurnOn();
        }
        public bool CanMakeCoffee()
        {
            return false;
        }
        public bool CanMakeCoffee(CoffeeMachineAction action)
        {
            action.AdditionalMessage = Reason;
            return CanMakeCoffee();
        }
    }

    class Idle : CoffeeMachineState
    {
        private string Reason = "Machine is active";
        public string DescriptionGet()
        {
            return "Idle";
        }
        public bool CanTurnOff()
        {
            return true;
        }
        public bool CanTurnOff(CoffeeMachineAction action)
        {
            return CanTurnOff();
        }
        public bool CanTurnOn()
        {
            return false;
        }
        public bool CanTurnOn(CoffeeMachineAction action)
        {
            action.AdditionalMessage = Reason;
            return CanTurnOn();
        }
        public bool CanMakeCoffee()
        {
            return true;
        }
        public bool CanMakeCoffee(CoffeeMachineAction action)
        {
            return CanMakeCoffee();
        }
    }

    class Active : CoffeeMachineState
    {
        private string Reason = "Machine is active";
        public string DescriptionGet()
        {
            return "Active";
        }
        public bool CanTurnOff()
        {
            return false;
        }
        public bool CanTurnOff(CoffeeMachineAction action)
        {
            action.AdditionalMessage = Reason;
            return CanTurnOff();
        }
        public bool CanTurnOn()
        {
            return false;
        }
        public bool CanTurnOn(CoffeeMachineAction action)
        {
            action.AdditionalMessage = Reason;
            return CanTurnOn();
        }
        public bool CanMakeCoffee()
        {
            return false;
        }
        public bool CanMakeCoffee(CoffeeMachineAction action)
        {
            action.AdditionalMessage = Reason;
            return CanMakeCoffee();
        }
    }

    class Alert : CoffeeMachineState
    {
        private string Reason = "Machine is alert";
        public string DescriptionGet()
        {
            return "Alert";
        }
        public bool CanTurnOff()
        {
            return true;
        }
        public bool CanTurnOff(CoffeeMachineAction action)
        {
            return CanTurnOff();
        }
        public bool CanTurnOn()
        {
            return false;
        }
        public bool CanTurnOn(CoffeeMachineAction action)
        {
            action.AdditionalMessage = Reason;
            return CanTurnOn();
        }
        public bool CanMakeCoffee()
        {
            return false;
        }
        public bool CanMakeCoffee(CoffeeMachineAction action)
        {
            action.AdditionalMessage = Reason;
            return CanMakeCoffee();
        }
    }
}
