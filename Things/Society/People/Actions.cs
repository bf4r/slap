namespace slap.Things.Society.People;

public partial class Person : Thing
{
    public bool Eat(Food food)
    {
        if (Fullness == 100) return false;
        Fullness += food.Nutrition;
        Thirst += food.Dryness;
        return true;
    }
    public bool Drink(Beverage beverage)
    {
        if (Hydration == 100) return false;
        Hydration += beverage.Hydration;
        return true;
    }
}
