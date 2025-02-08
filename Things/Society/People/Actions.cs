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
}
