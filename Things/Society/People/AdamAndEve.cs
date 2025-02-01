namespace slap.Things.Society.People;

using slap.Things.Society.People.Identity;

public partial class Person : Thing
{
    public static Person GetEve()
    {
        return new Person()
        {
            Conceived = Sim.Now - TimeSpan.FromDays(20 * 365),
            Born = Sim.Now - TimeSpan.FromDays(19 * 365),
            Gender = Gender.Female,
            FirstName = "Eve",
        };
    }
    public static Person GetAdam()
    {
        return new Person()
        {
            Conceived = Sim.Now - TimeSpan.FromDays(20 * 365),
            Born = Sim.Now - TimeSpan.FromDays(19 * 365),
            Gender = Gender.Male,
            FirstName = "Adam"
        };
    }
}
