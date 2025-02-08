namespace slap.Things.Society.People;

using slap.Things.Society.People.Identity;

public partial class Person : Thing
{
    public void Eat(Food food)
    {
        if (Fullness == 100)
        {
            Sim.Log.Failure($"{this.Who()} tried to eat {food.Description}, but couldn't, because {this.GetPronoun(PronounType.Subject)} is too full.");
            return;
        }
        Do(() =>
        {
            Fullness += food.Nutrition;
            Thirst += food.Dryness;
            Sim.Log.Success($"{this.Who()} ate {food.Description} and it was {food.Nutrition}% nutritious ({this.GetPronoun(PronounType.Subject)} is now {this.Hunger}% hungry) and made {this.GetPronoun(PronounType.Object)} {food.Dryness}% more thirsty (now {this.Thirst}%).");
        }, TimeSpan.FromMinutes(5));
    }
    public void Drink(Beverage beverage)
    {
        if (Hydration == 100)
        {
            Sim.Log.Failure($"{this.Who()} tried to drink {beverage.Description}, but couldn't, because {this.GetPronoun(PronounType.Subject)} is too hydrated.");
            return;
        }
        Do(() =>
        {
            Hydration += beverage.Hydration;
            Sim.Log.Success($"{this.Who()} drank {beverage.Description} and it quenched {this.GetPronoun(PronounType.PossessiveDeterminer)} thirst by {beverage.Hydration}% (now {this.Thirst}%).");
        }, TimeSpan.FromSeconds(10));
    }
    public void Run(int meters)
    {
        if (meters <= 0)
        {
            Sim.Log.Failure($"{this.Who()} tried to run a negative or zero distance.");
            return;
        }
        // How many meters to run to lower energy by 1%. The person can run 25 km at once max.
        var onePercentMeters = 250;
        double kmph = 15.0;
        TimeSpan duration = TimeSpan.FromHours(meters / 1000.0 / kmph);
        var takeEnergyPercent = meters / onePercentMeters;
        if (this.Energy < takeEnergyPercent)
        {
            Sim.Log.Failure($"{this.Who()} tried to run {Math.Round(meters / 1000.0, 1)} km, but couldn't, because {this.GetPronoun(PronounType.Subject)} is too tired for that length.");
            return;
        }
        Do(() =>
        {
            this.Energy -= takeEnergyPercent;
            Sim.Log.Success($"{this.Who()} ran {Math.Round(meters / 1000.0, 1)} km.");
        }, duration);
    }
    public void GiveMoney(Person recipient, decimal amount)
    {
        if (this.Money < amount)
        {
            Sim.Log.Failure($"{this.Who()} tried to pay {recipient.Who()} ${amount}, but only has {this.Money}.");
        }
        this.Money -= amount;
        recipient.Money += amount;
        Sim.Log.Success($"{this.Who()} gave ${amount} to {recipient.Who()}.");
    }
}
