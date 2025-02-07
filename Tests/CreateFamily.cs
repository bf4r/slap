namespace slap;

using slap.Things.Society.People;
using slap.Things.Society.People.Identity;

public static partial class Tests
{
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
}
