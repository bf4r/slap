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
        // Live life.
        UpdateStats();
        ActivateReflexes();

        if (ShouldWakeUp)
        {
            WakeUp();
        }
        DoQueuedActions();

        if (!IsSleeping && !IsDead)
        {
            if (this.Location == null) this.Location = new(null, null, 0, 0);
            if (Sim.Random.Next(100) == 0)
            {
                var randX = Sim.Random.Next(-1, 2);
                var randY = Sim.Random.Next(-1, 2);
                this.Location.X += randX;
                this.Location.Y += randY;
            }
        }

        CheckHealth();
    }
}
