namespace slap.Things.Society.People;

using slap.Things.Society.People.Identity;

public partial class Person : Thing
{
    public bool Eat(Food food)
    {
        if (Fullness == 100)
        {
            Sim.Log.Failure($"{this.GetDetails()} tried to eat {food.Description}, but couldn't, because {this.GetPronoun(PronounType.Subject)} is too full.");
            return false;
        }
        Fullness += food.Nutrition;
        Thirst += food.Dryness;
        Sim.Log.Success($"{this.GetDetails()} just ate {food.Description} and it was {food.Nutrition}% nutritious ({this.GetPronoun(PronounType.Subject)} is now {this.Hunger}% hungry) and made {this.GetPronoun(PronounType.Object)} {food.Dryness}% more thirsty (now {this.Thirst}%).");
        return true;
    }
    public bool Drink(Beverage beverage)
    {
        if (Hydration == 100)
        {
            Sim.Log.Failure($"{this.GetDetails()} tried to drink {beverage.Description}, but couldn't, because {this.GetPronoun(PronounType.Subject)} is too hydrated.");
            return false;
        }
        Hydration += beverage.Hydration;
        Sim.Log.Success($"{this.GetDetails()} just drank {beverage.Description} and it quenched {this.GetPronoun(PronounType.PossessiveDeterminer)} thirst by {beverage.Hydration}% (now {this.Thirst}%).");
        return true;
    }
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
