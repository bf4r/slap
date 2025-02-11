namespace slap.Things.Society.People;

public partial class Person : Thing
{
    private int _health = 100;
    private int _fullness = 100;
    private int _hydration = 100;
    private int _energy = 100;

    public int Health
    {
        get => _health;
        set => _health = Math.Clamp(value, 0, 100);
    }
    public int Fullness
    {
        get => _fullness;
        set => _fullness = Math.Clamp(value, 0, 100);
    }
    public int Hydration
    {
        get => _hydration;
        set => _hydration = Math.Clamp(value, 0, 100);
    }
    public int Energy
    {
        get => _energy;
        set => _energy = Math.Clamp(value, 0, 100);
    }

    // below are inverse properties for convenience in language:
    // 
    // Example:
    // If the person is hungry, Fullness will be 30 an Hunger will be 70
    // Hunger is not a hunger bar, it's a measure of how hungry the person is from 0 to 100
    // with 0 being the lowest and 100 being the most hungry
    // If hunger reaches 100, the person dies out of starvation
    public int Sickness
    {
        get => 100 - Health;
        set => Health = 100 - value;
    }
    public int Hunger
    {
        get => 100 - Fullness;
        set => Fullness = 100 - value;
    }
    public int Thirst
    {
        get => 100 - Hydration;
        set => Hydration = 100 - value;
    }
    public int Exhaustion
    {
        get => 100 - Energy;
        set => Energy = 100 - value;
    }

    public void CheckHealth()
    {
        if (Fullness == 0) Die("Starvation");
        if (Hydration == 0) Die("Dehydration");
        if (Energy == 0) Sleep();
        if (GetAgeYears() > 110) Die("Old age");
    }

    public static void CheckHealth(List<Person> people)
    {
        people.ForEach(p => p.CheckHealth());
    }

    // private DateTime _lastHealthTickDown; // Health doesn't tick down with time.
    private DateTime _lastEnergyTickDown = Sim.Now;
    private DateTime _lastFoodTickDown = Sim.Now;
    private DateTime _lastHydrationTickDown = Sim.Now;
    private DateTime _lastSlept = Sim.Now;
    private DateTime _lastSleptHours = Sim.Now;
    private void UpdateStats()
    {
        bool logStats = false;
        // 100 to 0 in 2 days (fullness).
        if (Sim.Now - _lastFoodTickDown > TimeSpan.FromSeconds(1728) && IsMetabolismActive)
        {
            _lastFoodTickDown = Sim.Now;
            Fullness--;
            if (logStats) Sim.Log.Info($"{this.Who()} is now {this.Hunger}% hungry.");
        }
        // 100 to 0 in 1 day (hydration).
        if (Sim.Now - _lastHydrationTickDown > TimeSpan.FromSeconds(864) && IsMetabolismActive)
        {
            _lastHydrationTickDown = Sim.Now;
            Hydration--;
            if (logStats) Sim.Log.Info($"{this.Who()} is now {this.Thirst}% thirsty.");
        }
        // 100 to 0 in 16 hours without sleep (energy).
        if (Sim.Now - _lastEnergyTickDown > TimeSpan.FromSeconds(576) && !IsSleeping)
        {
            _lastEnergyTickDown = Sim.Now;
            Energy--;
            if (logStats) Sim.Log.Info($"{this.Who()} is now {this.Exhaustion}% tired.");
        }
    }
}
