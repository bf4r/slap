namespace slap.Things.Society.People;

public partial class Person : Thing
{
    // allows defining how the person behaves, e.g. if the hunger is above 50%, eat something.
    private List<(Func<bool> condition, Action action)> Reflexes { get; set; } = new();
    public void DevelopReflex(Func<bool> condition, Action action)
    {
        Reflexes.Add((condition, action));
    }
    private void ActivateReflexes()
    {
        foreach ((var condition, var action) in Reflexes)
        {
            if (condition())
            {
                action();
            }
        }
    }
}
