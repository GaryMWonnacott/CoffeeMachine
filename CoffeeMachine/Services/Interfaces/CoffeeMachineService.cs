using Microsoft.Extensions.Caching.Memory;

namespace CoffeeMachine.Services.CoffeeMachine
{
    public enum State
    {
        Okay = 0,
        Alert = 1
    }
    public struct CoffeeCreationOptions
    {
        public int NumEspressoShots { get; set; }
        public bool AddMilk { get; set; }

        public CoffeeCreationOptions(int numEspressoShots, bool addMilk) : this()
        {
            this.NumEspressoShots = numEspressoShots;
            this.AddMilk = addMilk;
        }
    }

    public struct cacheNeverRemove
    {
        public cacheNeverRemove()
        {
            Option = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
        }
        public MemoryCacheEntryOptions Option { get; private set; }
    }
    public interface ICoffeeMachine
    {
        bool IsOn { get; }
        bool IsMakingCoffee { get; }
        State WaterLevelState { get; }
        State BeanFeedState { get; }
        State WasteCoffeeState { get; }
        State WaterTrayState { get; }
        Task TurnOnAsync();
        Task TurnOffAsync();
        Task MakeCoffeeAsync(CoffeeCreationOptions options);
    }

    public class CoffeeMachineStub : ICoffeeMachine
    {
        private readonly IMemoryCache _cache;
        public CoffeeMachineStub(IMemoryCache cache)
        {
            _cache = cache;
            _randomStateGenerator = new Random();
            _cacheOption = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
            
        }
        private MemoryCacheEntryOptions _cacheOption { get; set; }
        public bool IsOn { get { bool val; var exists = _cache.TryGetValue("IsOn",out val); return exists ? val : false; } private set { _cache.Set("IsOn", value, _cacheOption); } }
        public bool IsMakingCoffee { get { bool val; var exists = _cache.TryGetValue("IsMakingCoffee", out val); return exists ? val : false; } private set { _cache.Set("IsMakingCoffee", value, _cacheOption); } }
        public State WaterLevelState { get { State val; var exists = _cache.TryGetValue("WaterLevelState", out val); return exists ? val : State.Okay; } private set { _cache.Set("WaterLevelState", value, _cacheOption); } }
        public State BeanFeedState { get { State val; var exists = _cache.TryGetValue("BeanFeedState", out val); return exists ? val : State.Okay; } private set { _cache.Set("BeanFeedState", value, _cacheOption); } }
        public State WasteCoffeeState { get { State val; var exists = _cache.TryGetValue("WasteCoffeeState", out val); return exists ? val : State.Okay; } private set { _cache.Set("WasteCoffeeState", value, _cacheOption); } }
        public State WaterTrayState { get { State val; var exists = _cache.TryGetValue("WaterTrayState", out val); return exists ? val : State.Okay; } private set { _cache.Set("WaterTrayState", value, _cacheOption); } }
        private bool IsInAlertState => WaterLevelState == State.Alert
        || BeanFeedState == State.Alert
        || WasteCoffeeState == State.Alert
       || WaterTrayState == State.Alert;
        private readonly Random _randomStateGenerator;
        public async Task TurnOnAsync()
        {
            if (IsOn)
                throw new InvalidOperationException("Invalid state");
            // Generate sample state for testing
            WaterLevelState = GetRandomState();
            BeanFeedState = GetRandomState();
            WasteCoffeeState = GetRandomState();
            WaterTrayState = GetRandomState();
        // [Machine turned on]
            IsOn = true;
        }
        public async Task TurnOffAsync()
        {
            if (!IsOn || IsMakingCoffee)
                throw new InvalidOperationException("Invalid state");
            // [Machine turned off]
            IsOn = false;
        }

        public async Task MakeCoffeeAsync(CoffeeCreationOptions options)
        {
            if (!IsOn || IsMakingCoffee || IsInAlertState)
                throw new InvalidOperationException("Invalid state");
            IsMakingCoffee = true;
            // [Make the coffee]
            Thread.Sleep(10000);
            IsMakingCoffee = false;

        }
        // Randomly create a state for testing. This can be replaced as required.
        private State GetRandomState() => _randomStateGenerator.Next(1, 10) == 1 ? State.Alert : State.Okay;
    }
}
