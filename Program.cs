namespace slap;

using slap.Logging;
using slap.Things;
using slap.Things.Society;
using slap.Things.Society.Relationships;

class Program
{
    static void Main(string[] args)
    {
        Logger logger = new();
        Simulation.SetLogger(logger);
        try
        {
            Run(logger);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
        }
        logger.PrintLogs(useColors: true);
    }
    public static void Run(Logger log)
    {
        Thing thing = new Thing("Thing", "A thing.");
        log.Info($"A thing has been created, its name is {thing.Name ?? "unknown"}.");
        if (thing.Description != null) log.Info($"More about {thing.Name ?? "the unknown thing"}: {thing.Description}");
        else log.Warning($"The thing {thing.Name ?? "with an unknown name"} is missing a description.");

        Person person = new Person();
        person.Conceive();
        person.Birth();
        person.ReassignGender(Gender.Female);
        person.GiveName("Eve", "Smith");

        LogBaby(log, person);
        log.Info("Person details: " + person.GetDetails());

        Simulation.WaitYears(5);

        person.Move(Location.Get(CommonLocations.CommonCities.Paris));
        log.Info((person.FirstName ?? "An unnamed person") + " is now in " + (person.Location?.Name ?? "nowhere!"));

        Person person2 = new Person();
        person2.Conceive();
        person2.Birth();
        person2.ReassignGender(Gender.Male);
        person2.GiveName("Jonathan", "Lee");

        person2.Move(Location.Get(CommonLocations.CommonCities.Paris));
        log.Info((person2.FirstName ?? "An unnamed person") + " is now in " + (person2.Location?.Name ?? "nowhere!"));

        log.Sep();

        LogBaby(log, person2);

        Simulation.WaitYears(20);

        log.Info("There has been a murder.");
        log.Info("Killer details: " + person2.GetDetails());
        log.Info("Victim details: " + person.GetDetails());

        person2.Kill(person);
        log.Info("The victim is now " + (person.IsDead ? "dead" : "alive") + ".");
        log.Info("Victim details: " + person.GetDetails());
        log.Info("The killer is " + person.Killer?.GetDetails() + ".");

        log.Sep();

        // dating, breakup, dating, marriage, divorce
        var husband = new Person();
        husband.Conceive();
        husband.Birth();
        husband.GiveName("John", "Doe");
        LogBaby(log, husband);

        Simulation.WaitYears(5);

        var wife = new Person();
        wife.Conceive();
        wife.Birth();
        wife.GiveName("Jane", "Doe");
        LogBaby(log, wife);

        Simulation.WaitYears(24);

        log.Info($"{husband.GetDetails()} has asked out {wife.GetDetails()}.");
        AskOutOutcome outcome = husband.AskOut(wife);
        var message = outcome switch
        {
            AskOutOutcome.Accepted => $"{wife.FirstName} has accepted to date {husband.FirstName}.",
            AskOutOutcome.RejectedPreference => $"{wife.FirstName} rejected {husband.FirstName}.",
            AskOutOutcome.RejectedIncompatibleAge => $"{wife.FirstName} can't date {husband.FirstName} because of age incompatibility.",
            AskOutOutcome.RejectedIncompatibleSexuality => $"{wife.FirstName} isn't into {husband.FirstName}.",
            _ => "OOPS"
        };
        if (message == "OOPS") throw new NotImplementedException("Unknown AskOutOutcome!");
        log.Info(message);
        bool bothDating = (husband.IsInRelationshipWith(wife) && husband.RelationshipStatus == RelationshipStatus.Dating && wife.RelationshipStatus == RelationshipStatus.Dating);
        if (bothDating) log.Success($"{husband.FirstName} and {wife.FirstName} are now dating.");
        else log.Failure($"{husband.FirstName} and {wife.FirstName} are still single and not dating.");
        RelationshipStatus? matchingRelationshipStatus = null;
        if (husband.RelationshipStatus == wife.RelationshipStatus)
        {
            matchingRelationshipStatus = husband.RelationshipStatus;
        }
        if (matchingRelationshipStatus != null)
        {
            log.Success($"Their relationship status is matching. They are both {matchingRelationshipStatus.ToString()!.ToLower()}.");
        }
        else
        {
            log.Failure($"Their relationship status does not match.");
            log.Info($"{husband.FirstName}'s relationship status is {husband.RelationshipStatus.ToString()}.");
            log.Info($"{wife.FirstName}'s relationship status is {wife.RelationshipStatus.ToString()}.");
        }
    }
    static void LogBaby(Logger log, Person baby)
    {
        log.Info($"A new baby was born! Their name is {baby.GetFullName() ?? "unknown"}.");
    }
}
