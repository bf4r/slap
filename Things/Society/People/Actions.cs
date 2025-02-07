namespace slap.Things.Society.People;

public partial class Person
{
    public bool Eat(Food food)
    {
        if (Fullness == 100) return false;
        Fullness += food.Nutrition;
        Thirst += food.Dryness;
        return true;
    }
}
