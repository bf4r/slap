namespace slap;
using slap.Logging;
using slap.Things;
using slap.Things.Society;

public static class Tests
{
    public static void HelloWorldThing()
    {
        var thing = new Thing("Thing", "A thing");
        Sim.Log.Success($"A new thing has been created, its name is {thing.Name} and its description is \"{thing.Description}\".");
        Sim.WaitSeconds(5);
    }
    public static void SocietyTest()
    {
        var adam = Person.GetAdam();
        var eve = Person.GetEve();
        adam.MakeLove(eve);

        Sim.WaitMonths(9);
        Sim.RandomDayTime();

        var child = eve.GiveBirth();
        var childName = child.Gender == Gender.Male ? "Luke" : "Emma";
        eve.NameChild(child, childName);
        LogHelpers.LogBaby(child);
        Sim.WaitYears(18);
        child.Say($"Hello! I'm {child.GetFullName()} and I'm {child.GetAgeYears()}.");
    }
}
