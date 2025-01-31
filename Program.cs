namespace slap;

using slap.Logging;
using slap.Things;
using slap.Things.Society;
using slap.Things.Society.Relationships;

class Program
{
    // todo: move these helper methods somewhere else
    public static void LogBaby(Logger log, Person baby)
    {
        log.Info($"A new baby was born! {baby.GetPronoun(PronounType.PossessiveDeterminer).CapitalizeFirst()} name is {baby.GetFullName() ?? "unknown"}.");
    }
    public static void LogCoupleStatus(Logger log, Person person1, Person person2)
    {
        var rel = person1.IsInRelationshipWith(person2);
        log.Info($"Relationship status of {person1.GetDetails()} & {person2.GetDetails()}:");
        if (rel) log.Success($"They are in a relationship.");
        else log.Failure("They are not in a relationship.");
        RelationshipStatus? matchingRelationshipStatus = null;
        if (person1.RelationshipStatus == person2.RelationshipStatus)
        {
            matchingRelationshipStatus = person1.RelationshipStatus;
        }
        if (matchingRelationshipStatus != null)
        {
            log.Success($"Their relationship status is matching. They are both {matchingRelationshipStatus.ToString()!.ToLower()}.");
        }
        else
        {
            log.Failure($"Their relationship status does not match.");
            log.Info($"{person1.FirstName}'s relationship status is {person1.RelationshipStatus.ToString()}.");
            log.Info($"{person2.FirstName}'s relationship status is {person2.RelationshipStatus.ToString()}.");
        }
    }
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
        Simulation.RandomDayTime();
        Thing thing = new Thing("Thing", "A thing.");
        log.Info($"A thing has been created, its name is {thing.Name ?? "unknown"}.");
        if (thing.Description != null) log.Info($"More about {thing.Name ?? "the unknown thing"}: {thing.Description}");
        else log.Warning($"The thing {thing.Name ?? "with an unknown name"} is missing a description.");

        Person person = new Person();
        person.Conceive();
        person.Birth();
        person.AssignGender(Gender.Female);
        person.GiveName("Eve", "Smith");

        LogBaby(log, person);
        log.Info("Person details: " + person.GetDetails());

        Simulation.WaitYears(5);
        Simulation.RandomDayTime();

        person.Move(Location.Get(CommonLocations.CommonCities.Paris));
        log.Info((person.FirstName ?? "An unnamed person") + " is now in " + (person.Location?.Name ?? "nowhere!"));

        Person person2 = new Person();
        person2.Conceive();
        person2.Birth();
        person2.AssignGender(Gender.Male);
        person2.GiveName("Jonathan", "Lee");

        person2.Move(Location.Get(CommonLocations.CommonCities.Paris));
        log.Info((person2.FirstName ?? "An unnamed person") + " is now in " + (person2.Location?.Name ?? "nowhere!"));

        log.Sep();

        LogBaby(log, person2);

        Simulation.WaitYears(20);
        Simulation.RandomDayTime();

        log.Info("There has been a murder.");
        log.Info("Killer details: " + person2.GetDetails());
        log.Info("Victim details: " + person.GetDetails());

        person2.Kill(person);
        log.Info("The victim is now " + (person.IsDead ? "dead" : "alive") + ".");
        log.Info("Victim details: " + person.GetDetails());
        log.Info("The killer is " + person.Killer?.GetDetails() + ".");

        log.Sep();

        var officiant = new Person();
        officiant.Conceive();
        officiant.Birth();
        officiant.AssignGender(Gender.Male);
        officiant.GiveName("Paul", "Smith");
        LogBaby(log, officiant);

        Simulation.WaitYears(10);
        Simulation.RandomDayTime();

        // dating, breakup, dating, marriage, divorce
        var husband = new Person();
        husband.Conceive();
        husband.Birth();
        husband.AssignGender(Gender.Male);
        husband.GiveName("John", "Doe");
        LogBaby(log, husband);

        Simulation.WaitYears(5);
        Simulation.RandomDayTime();

        var wife = new Person();
        wife.Conceive();
        wife.Birth();
        wife.AssignGender(Gender.Female);
        wife.GiveName("Jane", "Parker");
        LogBaby(log, wife);


        Simulation.WaitYears(24);
        Simulation.RandomDayTime();

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
        LogCoupleStatus(log, husband, wife);
        Location weddingLocation = new(
                name: "Church of Slap",
                description: "The church where the wedding takes place.",
                latitude: 0,
                longitude: 0
                );
        if (bothDating)
        {
            Simulation.WaitYears(1);
            Simulation.RandomDayTime();
            log.Info($"{husband.FirstName} is proposing to {wife.FirstName}.");
            Simulation.Wait(TimeSpan.FromSeconds(5));
            husband.Say(log, $"{wife.FirstName}, will you marry me?");
            Simulation.Wait(TimeSpan.FromSeconds(4));
            bool wifeSaidYes = husband.Propose(wife);
            // wedding
            if (wifeSaidYes)
            {
                wife.Say(log, "Yes!");
                LogCoupleStatus(log, husband, wife);
                var tempHusbandLocation = husband.Location;
                var tempWifeLocation = wife.Location;
                var tempOfficiantLocation = officiant.Location;
                officiant.Move(weddingLocation);
                Simulation.Wait(TimeSpan.FromHours(1) + TimeSpan.FromMinutes(30));
                husband.Move(weddingLocation);
                wife.Move(weddingLocation);
                bool weddingSuccessful = husband.Marry(wife);
                if (weddingSuccessful)
                {
                    officiant.PreferredName = $"Officiant {officiant.FirstName}";
                    officiant.Say(log, $"I now pronounce you {(husband.Gender == Gender.Male ? "husband" : "wife")} and {(wife.Gender == Gender.Male ? "husband" : "wife")}! You may now kiss.");
                    // wife.Kiss(husband);
                    LogCoupleStatus(log, husband, wife);
                    Simulation.RandomDayTime();
                    officiant.PreferredName = officiant.FirstName;
                    Simulation.WaitYears(12);
                    Simulation.RandomDayTime();
                    wife.Say(log, $"We're done, {husband.FirstName}.");
                    wife.Divorce(husband);
                    log.Info($"{wife.FirstName} has divorced {husband.FirstName}.");
                }
                else
                {
                    Simulation.Wait(TimeSpan.FromHours(1) + TimeSpan.FromMinutes(30));
                    husband.Location = tempHusbandLocation;
                    wife.Location = tempWifeLocation;
                }
            }
            else
            {
                wife.Say(log, "No. I'm sorry.");
                LogCoupleStatus(log, husband, wife);
                Simulation.Wait(TimeSpan.FromHours(1));
                bool breakUp = Simulation.Random.Next(0, 2) == 0;
                if (breakUp)
                {
                    wife.Say(log, $"{husband.FirstName}, I'm breaking up with you.");
                    wife.BreakUp(husband);
                    // wife.Cry();
                    // husband.Cry();
                }
            }
            LogCoupleStatus(log, husband, wife);
            log.Sep();
            log.Info($"Things said by {wife.GetDetails()}:");
            wife.PrintAllThingsSaid(log, useColors: true);
            log.Sep();
        }
    }
}
