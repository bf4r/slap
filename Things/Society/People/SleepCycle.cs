namespace slap.Things.Society.People;

public partial class Person : Thing
{
    public DateTime? _lastWentToSleep = null;
    public int SleepsHours { get; set; } = 8;
    public bool ShouldWakeUp => Sim.Now - _lastWentToSleep > TimeSpan.FromHours(SleepsHours) && IsSleeping;
    public bool IsSleeping { get; set; }
    public bool IsMetabolismActive { get; set; } = true;
    public void StopMetabolism()
    {
        IsMetabolismActive = false;
    }
    public void ResumeMetabolism()
    {
        IsMetabolismActive = true;
    }
    private void StartSleeping()
    {
        SleepsHours = Exhaustion / 10 + Sim.Random.Next(-2, 2 + 1);
        _lastWentToSleep = Sim.Now;
        IsSleeping = true;
        StopMetabolism();
    }
    public void WakeUp()
    {
        IsSleeping = false;
        ResumeMetabolism();
        var diff = (Sim.Now - _lastWentToSleep);
        if (diff == null)
        {
            Sim.Log.Success($"{this.Who()} woke up.");
            this.Energy = 100;
        }
        else
        {
            var hours = diff.Value.Hours;
            this.Energy += hours * 10;
            Sim.Log.Success($"{this.Who()} woke up after {hours} hours of sleep.");
        }
    }
    public void Sleep()
    {
        if (IsSleeping) return;
        if (Energy > 90)
        {
            Sim.Log.Failure($"{this.Who()} tried to go to sleep but has too much energy ({this.Energy}%).");
            return;
        }
        Do(() =>
        {
            StartSleeping();
            Sim.Log.Success($"{this.Who()} went to sleep.");
        }, TimeSpan.FromHours(SleepsHours));
        return;
    }
}
