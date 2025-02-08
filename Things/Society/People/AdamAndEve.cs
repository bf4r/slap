namespace slap.Things.Society.People;

using slap.Things.Society.People.Identity;

public partial class Person : Thing
{
    private static DateTime _conception = Sim.Now - TimeSpan.FromDays(20 * 365) + TimeSpan.FromSeconds(Sim.Random.Next(0, 86400));
    public static Person GetEve()
    {
        return new Person()
        {
            Conceived = _conception,
            Born = _conception + TimeSpan.FromDays(30 * 9) + TimeSpan.FromSeconds(Sim.Random.Next(0, 86400)),
            Gender = Gender.Female,
            FirstName = "Eve",
        };
    }
    public static Person GetAdam()
    {
        return new Person()
        {
            Conceived = _conception,
            Born = _conception + TimeSpan.FromDays(30 * 9) + TimeSpan.FromSeconds(Sim.Random.Next(0, 86400)),
            Gender = Gender.Male,
            FirstName = "Adam"
        };
    }
}
