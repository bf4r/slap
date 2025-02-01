namespace slap.Things.Society.People;

public partial class Person : Thing
{
    private int _health;
    private int _fullness;
    private int _hydration;
    private int _energy;

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

        // once Sleep() is added
        // if (Energy == 0) Sleep("Exhaustion");
    }

    public static void CheckHealth(List<Person> people)
    {
        people.ForEach(p => p.CheckHealth());
    }
}
