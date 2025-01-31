namespace slap.Logging;

using slap.Things.Society;
using slap.Things.Society.Relationships;

public static class LogHelpers
{
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
}
