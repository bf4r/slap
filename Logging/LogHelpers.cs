namespace slap.Logging;

using slap.Things.Society.People;
using slap.Things.Society.People.Identity;
using slap.Things.Society.People.Relationships;

public static class LogHelpers
{
    public static void LogBaby(Person baby)
    {
        Sim.Log.Info($"A new baby was born! {baby.GetPronoun(PronounType.PossessiveDeterminer).CapitalizeFirst()} name is {baby.GetFullName() ?? "unknown"}.");
    }
    public static void LogCoupleStatus(Person person1, Person person2)
    {
        var rel = person1.IsInRelationshipWith(person2);
        Sim.Log.Info($"Relationship status of {person1.GetDetails()} & {person2.GetDetails()}:");
        if (rel) Sim.Log.Success($"They are in a relationship.");
        else Sim.Log.Failure("They are not in a relationship.");
        RelationshipStatus? matchingRelationshipStatus = null;
        if (person1.RelationshipStatus == person2.RelationshipStatus)
        {
            matchingRelationshipStatus = person1.RelationshipStatus;
        }
        if (matchingRelationshipStatus != null)
        {
            Sim.Log.Success($"Their relationship status is matching. They are both {matchingRelationshipStatus.ToString()!.ToLower()}.");
        }
        else
        {
            Sim.Log.Failure($"Their relationship status does not match.");
            Sim.Log.Info($"{person1.FirstName}'s relationship status is {person1.RelationshipStatus.ToString()}.");
            Sim.Log.Info($"{person2.FirstName}'s relationship status is {person2.RelationshipStatus.ToString()}.");
        }
    }
}
