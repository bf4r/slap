namespace slap.Things.Society.People;

public partial class Person : Thing
{
    public new void Update()
    {
        if (!IsBorn) return;
        if (IsDead)
        {
            return;
        }
        UpdateStats();
        ActivateReflexes();

        if (ShouldWakeUp)
        {
            WakeUp();
        }
        DoQueuedActions();
        Move();

        CheckHealth();
    }
}
