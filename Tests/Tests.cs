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
        Food bread = new Food("Bread", "a slice of bread", nutrition: 10, dryness: 10);
        Beverage water = new Beverage("Water", "a glass of water", hydration: 20);

        (Person adam, Person eve, Person child) = CreateInitialFamily();
        Sim.Log.Success($"The initial family with {adam.GetDetails()}, {eve.GetDetails()} and their child {child.GetDetails()} has been created.");

        List<Person> fam = [adam, eve, child];
        foreach (var person in fam)
        {
            person.LastName = "Smith";
            person.DevelopReflex("eating", () => person.Hunger >= 60, () => person.Eat(bread));
            person.DevelopReflex("drinking", () => person.Thirst >= 50, () => person.Drink(water));
            person.DevelopReflex("sleeping", () => person.Energy <= 20 && Sim.Now.Hour > 20 || Sim.Now.Hour < 2, () => person.Sleep());
        }
        StartSimulation();
    }
    public static void StartSimulation()
    {
        // Speed up time because otherwise it would be kinda boring and slow.
        // Set it to 1 for real-time.
        Sim.SetTimeSpeed(5000);
        Sim.UpdateFrequency = TimeSpan.FromMilliseconds(1);
        Sim.Log.Info($"Starting simulation. Current time speed: {Sim.CurrentSpeedFactor}x");
        Sim.Run();
    }
}
