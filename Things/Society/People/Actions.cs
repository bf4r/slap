namespace slap.Things.Society.People;

using slap.Things.Society.People.Identity;

public partial class Person : Thing
{
    public void Eat(Food food)
    {
        if (Fullness == 100)
        {
            Sim.Log.Failure($"{this.GetDetails()} tried to eat {food.Description}, but couldn't, because {this.GetPronoun(PronounType.Subject)} is too full.");
            return;
        }
        Do(() =>
        {
            Fullness += food.Nutrition;
            Thirst += food.Dryness;
            Sim.Log.Success($"{this.GetDetails()} just ate {food.Description} and it was {food.Nutrition}% nutritious ({this.GetPronoun(PronounType.Subject)} is now {this.Hunger}% hungry) and made {this.GetPronoun(PronounType.Object)} {food.Dryness}% more thirsty (now {this.Thirst}%).");
        }, TimeSpan.FromMinutes(5));
    }
    public void Drink(Beverage beverage)
    {
        if (Hydration == 100)
        {
            Sim.Log.Failure($"{this.GetDetails()} tried to drink {beverage.Description}, but couldn't, because {this.GetPronoun(PronounType.Subject)} is too hydrated.");
            return;
        }
        Do(() =>
        {
            Hydration += beverage.Hydration;
            Sim.Log.Success($"{this.GetDetails()} just drank {beverage.Description} and it quenched {this.GetPronoun(PronounType.PossessiveDeterminer)} thirst by {beverage.Hydration}% (now {this.Thirst}%).");
        }, TimeSpan.FromSeconds(10));
    }
}
