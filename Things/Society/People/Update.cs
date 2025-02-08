namespace slap.Things.Society.People;

public partial class Person : Thing
{
    public new void Update()
    {
        if (!IsBorn) return;
        if (IsDead)
        {
            Sim.Stuff.Remove(this);
            return;
        }
        // Live life.
        UpdateStats();
        ActivateReflexes();

        if (ShouldWakeUp)
        {
            WakeUp();
        }
        DoQueuedActions();

        CheckHealth();
    }
}
