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

            person.DevelopReflex("morning_exercise", () =>
                Sim.Now.Hour == 6 &&
                person.Energy > 50 &&
                person.Health < 90,
                () => person.Run(2000)
            );

            person.DevelopReflex("family_chat", () =>
                Sim.Now.Hour >= 19 &&
                Sim.Now.Hour <= 21 &&
                familyMembers.Any(m => m != person && m.IsNearby(person)),
                () => person.Chat(familyMembers.First(m => m != person && m.IsNearby(person)))
            );

            person.DevelopReflex("allowance", () =>
                person.Money > 100 &&
                familyMembers.Any(m => m != person && m.Money < 20) &&
                (decimal)Sim.Random.NextDouble() < (person.Money / 1000.0m),
                () =>
                {
                    var needyFamilyMember = familyMembers
                        .Where(m => m != person && m.Money < 20)
                        .OrderBy(m => m.Money)
                        .First();

                    var shareAmount = Math.Min(
                        person.Money * 0.3m,
                        Math.Max(20, person.Money / 10)
                    );

                    person.GiveMoney(needyFamilyMember, shareAmount);
                }
            );

            person.DevelopReflex("wander", () =>
                person.Energy > 20 &&
                !person.IsSleeping &&
                Sim.Random.Next(100) < 10,
                () => person.Move()
            );

            person.DevelopReflex("morning_routine", () =>
                Sim.Now.Hour == 7 &&
                person.Energy > 50,
                () => person.MorningRoutine()
            );

            person.DevelopReflex("rest_when_sick", () =>
                person.Health < 70 &&
                person.Energy < 50,
                () => person.Rest()
            );

            person.Energy = Sim.Random.Next(80, 100);
            person.Fullness = Sim.Random.Next(80, 100);
            person.Hydration = Sim.Random.Next(80, 100);
        }
        List<House> houses = [];
        for (int i = -10; i < 10; i++)
        {
            houses.Add(new(new Location(i * 40, 0)));
        }
        StartSimulation();
    }
    public static void StartSimulation()
    {
        // Set it to 1 for real-time.
        // Set it to 2 for 2x speed.
        Sim.Speed = 1;

        // Prevent delay after a long time.
        Sim.Log.MessageLimit = 20000;

        // In real time, how long to wait until the state is updated with what happened.
        Sim.UpdateFrequency = TimeSpan.FromMilliseconds(10);

        // Start the simulation.
        Sim.Log.Info($"Starting simulation. Current time speed: {Sim.Speed}x");
        Sim.Run();
    }
}
