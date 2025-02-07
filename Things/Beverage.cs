namespace slap.Things;

public class Beverage : Thing
{
    public int Hydration { get; set; }
    public Beverage(string? name, string? description, int hydration) : base(name, description)
    {
        Hydration = hydration;
    }
}
