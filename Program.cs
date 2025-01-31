namespace slap;

using slap.Logging;
using slap.Things;
using slap.Things.Society;

class Program
{
    static void Main(string[] args)
    {
        Logger logger = new();
        try
        {
            Run(logger);
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, ex.Message);
        }
        logger.PrintLogs();
    }
    public static void Run(Logger logger)
    {
        Thing thing = new Thing("Thing", "A thing.");
        logger.Log(LogLevel.Info, $"A thing has been created, its name is {thing.Name ?? "unknown"}.");
        if (thing.Description != null)
        {
            logger.Log(LogLevel.Info, $"More about {thing.Name ?? "the unknown thing"}: {thing.Description}");
        }
        else
        {
            logger.Log(LogLevel.Warning, $"The thing {thing.Name ?? "with an unknown name"} is missing a description.");
        }

        Person person = new Person();
        person.Conceive();
        person.Birth();
        person.ReassignGender(Gender.Female);
        person.GiveName("Eve", "Smith");

        logger.Log(LogLevel.Info, $"A new baby was born! Their name is {person.GetFullName() ?? "Unknown name"} and they are {person.GetAgeYears()} years old.");
        logger.Log(LogLevel.Info, "Person details: " + person.GetDetails());

        logger.Log(LogLevel.Info, "Travelling 5 years into the future...");
        Simulation.Wait(TimeSpan.FromDays(5 * 365));
        logger.Log(LogLevel.Info, "Done!");

        person.Move(Location.Get(CommonLocations.CommonCities.Paris));
        logger.Log(LogLevel.Info, (person.FirstName ?? "An unnamed person") + " is now in " + (person.Location?.Name ?? "nowhere!"));

        Person person2 = new Person();
        person2.Conceive();
        person2.Birth();
        person2.ReassignGender(Gender.Male);
        person2.GiveName("Jonathan", "Lee");

        person2.Move(Location.Get(CommonLocations.CommonCities.Paris));
        logger.Log(LogLevel.Info, (person2.FirstName ?? "An unnamed person") + " is now in " + (person2.Location?.Name ?? "nowhere!"));

        logger.Log(LogLevel.Info, $"A new baby was born! Their name is {person2.GetFullName() ?? "Unknown name"} and they are {person2.GetAgeYears()} years old.");

        logger.Log(LogLevel.Info, "Travelling 20 years into the future...");
        Simulation.Wait(TimeSpan.FromDays(20 * 365));
        logger.Log(LogLevel.Info, "Done!");


        logger.Log(LogLevel.Info, "There has been a murder.");
        logger.Log(LogLevel.Info, "Killer details: " + person2.GetDetails());
        logger.Log(LogLevel.Info, "Victim details: " + person.GetDetails());

        person2.Kill(person);
        logger.Log(LogLevel.Info, "The victim is now " + (person.IsDead ? "dead" : "alive") + ".");
        logger.Log(LogLevel.Info, "Victim details: " + person.GetDetails());
        logger.Log(LogLevel.Info, "The killer is " + person.Killer?.GetDetails() + ".");
    }
}
