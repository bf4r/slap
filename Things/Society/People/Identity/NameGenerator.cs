namespace slap.Things.Society.People.Identity;

public static class NameGenerator
{
    private static readonly string[] MaleNames =
    {
        "James", "John", "William", "Michael", "David", "Daniel", "Joseph", "Thomas",
        "Christopher", "Robert", "Richard", "Charles", "Matthew", "Anthony", "Mark", "Luke"
    };

    private static readonly string[] FemaleNames =
    {
        "Mary", "Emma", "Elizabeth", "Sarah", "Margaret", "Emily", "Alice", "Helen",
        "Anna", "Laura", "Catherine", "Victoria", "Sophie", "Isabella", "Grace", "Olivia"
    };

    public static string GetRandomName(Gender gender)
    {
        return gender == Gender.Male
            ? MaleNames[Random.Shared.Next(MaleNames.Length)]
            : FemaleNames[Random.Shared.Next(FemaleNames.Length)];
    }
}
