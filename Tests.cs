namespace slap;

using slap.Things;
using slap.Things.Society.People;
using slap.Things.Society.People.Identity;

public static class Tests
{
    public static void ThingTest()
    {
        var thing = new Thing("Thing", "a thing");
        Sim.Log.Success($"A new thing has been created, its name is {thing.Name} and its description is \"{thing.Description}\".");
        Sim.WaitSeconds(5);
    }
    private static bool _initialFamilyCreated = false;
    // we want this function to only be used once
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
    public static void PeopleTest()
    {
        (Person adam, Person eve, Person child) = CreateInitialFamily();
        Sim.Log.Success($"The initial family with {adam.GetDetails()}, {eve.GetDetails()} and their child {child.GetDetails()} has been created.");
        if (child.IsRelatedTo(eve))
        {
            child.Say($"{eve.FirstName} is related to me!");
        }
        var bread = new Food("Bread", "a slice of bread", 10, 10);
        var ateBread = child.Eat(bread);
        if (ateBread)
            Sim.Log.Success($"{child.FirstName} ate {bread.Description}.");
        else
            Sim.Log.Failure($"{child.FirstName} is currently too full to eat {bread.Description}.");
    }
    public static void LocationTest()
    {
        var location = Location.GetRandomLocation();
        location.Name = "My random location";
        location.Description = "This is my random location. Welcome!";
        Sim.Log.Success($"A new location has been created called \"{location.Name}\".");
        Sim.Log.Info($"Location description: \"{location.Description}\".");
    }
}
