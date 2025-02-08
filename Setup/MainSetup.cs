namespace slap;

using slap.Things;
using slap.Things.Society.People;
using slap.Things.Society.People.Identity;

public static class MainSetup
{
    //
    // Add your simulation rules here.
    // Don't forget to call StartSimulation() at the end!
    // 
    public static void Run()
    {
        // items that will be used
        Food bread = new Food("Bread", "a slice of bread", nutrition: 20, dryness: 10);
        Beverage water = new Beverage("Water", "a glass of water", hydration: 15);

        (Person adam, Person eve, Person child) = CreateInitialFamily();
        Sim.Log.Success($"The initial family with {adam.Who()}, {eve.Who()} and their child {child.Who()} has been created.");

        List<Person> fam = [adam, eve, child];
        foreach (var person in fam)
        {
            person.LastName = "Smith";
            person.DevelopReflex("eating", () => person.Hunger >= 60, () => person.Eat(bread));
            person.DevelopReflex("drinking", () => person.Thirst >= 40, () => person.Drink(water));
            person.DevelopReflex("sleeping", () =>
                (Sim.Now.Hour > 20 && person.Energy < 20) ||
                (Sim.Now.Hour > 22 && person.Energy < 40) ||
                (person.Energy < 10) ||
                (Sim.Now.Hour >= 14 && Sim.Now.Hour <= 16 && person.Energy < 15) ||
                (Sim.Now.Hour >= 23 && person.Energy < 50) ||
                (person.Health < 50 && person.Energy < 30),

                () => person.Sleep()
            );
            person.Energy = Sim.Random.Next(80, 100);
            person.Fullness = Sim.Random.Next(80, 100);
            person.Hydration = Sim.Random.Next(80, 100);
        }
        StartSimulation();
    }
    public static void StartSimulation()
    {
        // Speed up time, because otherwise it would be kinda boring and slow.
        // Set it to 1 for real-time.
        Sim.SetTimeSpeed(512);

        // In real time, how long to wait until the state is updated with what happened.
        Sim.UpdateFrequency = TimeSpan.FromMilliseconds(20);

        // Start the simulation.
        Sim.Log.Info($"Starting simulation. Current time speed: {Sim.CurrentSpeedFactor}x");
        Sim.Run();
    }

    // You probably don't want to modify anything below this line.
    // ------------------------------------------------------------

    private static bool _initialFamilyCreated = false;

    // We want this function to only be used once!
    private static (Person adam, Person eve, Person child) CreateInitialFamily()
    {
        if (_initialFamilyCreated) throw new Exception("The initial family has already been created.");
        var adam = Person.GetAdam();
        var eve = Person.GetEve();
        adam.MakeLove(eve);
        Sim.WaitMonths(9);
        Sim.RandomDayTime();
        var child = eve.GiveBirth();
        var childName = child.Gender == Gender.Male ? "Luke" : "Emma";
        eve.NameChild(child, childName);
        Sim.WaitYears(18);
        Sim.RandomDayTime();
        _initialFamilyCreated = true;
        return (adam, eve, child);
    }
}
