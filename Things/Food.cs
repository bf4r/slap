namespace slap.Things;

public class Food : Thing
{
    public int Nutrition { get; set; }
    public int Dryness { get; set; }
    public Food(string? name, string? description, int nutrition, int dryness) : base(name, description)
    {
        Nutrition = nutrition;
        Dryness = dryness;
    }
}
