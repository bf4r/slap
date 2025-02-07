namespace slap.Things.Society.People;

public partial class Person : Thing
{
    // Allows for defining how the person behaves, e.g. if the hunger is above 50%, eat something.
    private List<Reflex> Reflexes { get; set; } = new();
    public void DevelopReflex(string name, Func<bool> condition, Action action)
    {
        if (Reflexes.Any(x => x.Name == name))
        {
            throw new Exception("Reflex names must be unique for each Person. To change a reflex, use ChangeReflex().");
        }
        Reflexes.Add(new(name, condition, action));
    }
    public void ChangeReflex(string name, Func<bool> condition, Action action)
    {
        if (!Reflexes.Any(x => x.Name == name))
        {
            throw new Exception($"The person does not have a reflex called {name}.");
        }
        Reflexes.Add(new(name, condition, action));
    }
    private void ActivateReflexes()
    {
        foreach (var reflex in Reflexes)
        {
            if (reflex.Condition())
            {
                reflex.Action();
            }
        }
    }
}

public record Reflex(string Name, Func<bool> Condition, Action Action);
