namespace slap;

using slap.Things;
using slap.Things.Society.People;

public static partial class Tests
{
    //
    // Add your simulation rules here.
    // Don't forget to call StartSimulation() at the end!
    // 
    public static void MainTest()
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
        // Speed up time because otherwise it would be kinda boring and slow.
        // Set it to 1 for real-time.
        Sim.SetTimeSpeed(500);

        // In real time, how long to wait until new logs are printed.
        Sim.UpdateFrequency = TimeSpan.FromMilliseconds(1);

        // Start the simulation.
        Sim.Log.Info($"Starting simulation. Current time speed: {Sim.CurrentSpeedFactor}x");
        Sim.Run();
    }
}
