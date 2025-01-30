namespace slap;

using slap.Logging;
using slap.Things;
using slap.Things.Society;

class Program
{
    static void Main(string[] args)
    {
        var logger = new Logger();
        Thing thing = new Thing("Thing", "Just a thing.");
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
        person.GiveName("Eve", "Smith");

        logger.Log(LogLevel.Info, $"A new baby was born! Their name is {person.GetFullName() ?? "Unknown name"} and they are {person.GetAgeYears()} years old.");
        logger.Log(LogLevel.Info, "Person details: " + person.GetDetails());

        logger.PrintLogs();
    }
}
