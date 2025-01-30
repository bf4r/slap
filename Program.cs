namespace slap;
using slap.Logging;

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
        logger.PrintLogs();
    }
}
