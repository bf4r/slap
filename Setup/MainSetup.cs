namespace slap;

using slap.Things;
using slap.Things.Society.People;

public static class MainSetup
{
    //
    // Add your simulation rules here.
    // Don't forget to call StartSimulation() at the end!
    // 
    public static void Run()
    {
        // Items that will be used.
        Food bread = new Food("Bread", "a slice of bread", nutrition: 20, dryness: 10);
        Beverage water = new Beverage("Water", "a glass of water", hydration: 15);

        // Create a family.
        var (father, mother, children) = FamilyCreator.CreateFamily(
            numberOfChildren: 50,
            lastName: "Smith",
            minChildAge: 5,
            maxChildAge: 15
        );

        Sim.Log.Success($"A new family has been created:");
        Sim.Log.Success($"Parents: {father.Who()} and {mother.Who()}");
        Sim.Log.Success($"Children: {string.Join(", ", children.Select(c => c.Who()))}");

        List<Person> familyMembers = [father, mother, .. children];

        foreach (var person in familyMembers)
        {
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
        // Set it to 0 for real-time.
        // Set it to 1 for 2x.
        // Set it to 500 for 499x.
        Sim.SetTimeSpeed(2000);

        // In real time, how long to wait until the state is updated with what happened.
        Sim.UpdateFrequency = TimeSpan.FromMilliseconds(20);

        // Start the simulation.
        Sim.Log.Info($"Starting simulation. Current time speed: {Sim.CurrentSpeedFactor}x");
        Sim.Run();
    }
}
