namespace slap.Things.Society.People;
using slap.Logging;

public partial class Person : Thing
{
    public List<LogMessage>? ThingsSaid { get; set; }
    public void Say(string message)
    {
        if (IsDead) throw new Exception("The person is dead and therefore cannot speak.");
        if (ThingsSaid != null)
        {
            if (GetAgeYears() < 1) message = "Goo goo ga ga!";
            ThingsSaid.Add(new LogMessage(LogLevel.Dialogue, $"{PreferredName}: \"{message}\""));
            Sim.Log.Dialogue($"{PreferredName}: \"{message}\"");
        }
    }
    public void LogAllThingsSaid()
    {
        if (ThingsSaid != null)
        {
            Console.WriteLine(ThingsSaid.Count);
            foreach (var message in ThingsSaid)
            {
                message.Log();
            }
        }
    }
}
