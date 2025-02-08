namespace slap.Things.Society.People;

public partial class Person : Thing
{
    public DateTime? _lastWentToSleep = null;
    public bool ShouldWakeUp => Sim.Now - _lastWentToSleep > TimeSpan.FromHours(8) && IsSleeping;
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
            Sim.Log.Success($"{this.GetDetails()} woke up.");
            this.Energy = 100;
        }
        else
        {
            var hours = diff.Value.Hours;
            this.Energy += hours * 10;
            Sim.Log.Success($"{this.GetDetails()} woke up after {hours} hours of sleep.");
        }
    }
    public bool Sleep()
    {
        if (IsSleeping) return false;
        if (Energy > 90)
        {
            Sim.Log.Failure($"{this.GetDetails()} tried to go to sleep but has too much energy ({this.Energy}%).");
            return false;
        }
        StartSleeping();
        Sim.Log.Success($"{this.GetDetails()} went to sleep.");
        return true;
    }
}
