namespace slap.Things.Society.People;

using slap.Things.Society.People.Identity;

public static class FamilyCreator
{
    private static readonly List<string> LastNames =
    [
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis",
        "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson"
    ];

    public static string GetRandomLastName()
    {
        return LastNames[Random.Shared.Next(LastNames.Count)];
    }

    public static (Person parent1, Person parent2, List<Person> children) CreateFamily(
        int numberOfChildren = 1,
        string? lastName = null,
        int minChildAge = 0,
        int maxChildAge = 18)
    {
        lastName ??= GetRandomLastName();

        var parent1 = Person.GetAdam();
        var parent2 = Person.GetEve();

        parent1.LastName = lastName;
        parent2.LastName = lastName;

        var children = new List<Person>();

        for (int i = 0; i < numberOfChildren; i++)
        {
            parent1.MakeLove(parent2);
            Sim.WaitMonths(9);
            Sim.RandomDayTime();

            var child = parent2.GiveBirth();
            var childName = NameGenerator.GetRandomName(child.Gender);
            parent2.NameChild(child, childName);
            child.LastName = lastName;
            if (Sim.Random.Next(0, 100) < 20)
            {
                child.LastName = GetRandomLastName();
            }

            children.Add(child);
        }

        if (maxChildAge > 0)
        {
            int yearsToAge = Random.Shared.Next(minChildAge, maxChildAge + 1);
            if (yearsToAge > 0)
            {
                Sim.WaitYears(yearsToAge);
                Sim.RandomDayTime();
            }
        }

        return (parent1, parent2, children);
    }
}
