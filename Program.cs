namespace slap;

using slap.Logging;
using slap.Things;
using slap.Things.Society;

class Program
{
    static void Main(string[] args)
    {
        var logger = new Logger();
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

        Person person2 = new Person();
        person2.Conceive();
        person2.Birth();
        person2.ReassignGender(Gender.Male);
        person2.GiveName("Jonathan", "Lee");

        logger.Log(LogLevel.Info, $"A new baby was born! Their name is {person2.GetFullName() ?? "Unknown name"} and they are {person2.GetAgeYears()} years old.");

        logger.Log(LogLevel.Info, "There has been a murder.");
        logger.Log(LogLevel.Info, "Killer details: " + person2.GetDetails());
        logger.Log(LogLevel.Info, "Victim details: " + person.GetDetails());

        person2.Kill(person);
        logger.Log(LogLevel.Info, "The victim is now " + (person.IsDead ? "dead" : "alive") + ".");
        logger.Log(LogLevel.Info, "Victim details: " + person.GetDetails());
        logger.Log(LogLevel.Info, "The killer is " + person.Killer?.GetDetails() + ".");

        logger.PrintLogs();
    }
}
