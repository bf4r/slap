namespace slap.Things;

public class House : Thing
{
    public string Texture { get; set; }
    public ConsoleColor Color { get; set; }
    public House(string texture, ConsoleColor color, Location location) : base(location: location)
    {
        Texture = texture;
        Color = color;
    }
    public House(ConsoleColor color, Location location) : base(location: location)
    {
        Texture = _defaultTexture;
    }
    public House(Location location) : base(location: location)
    {
        Texture = _defaultTexture;
        Color = ConsoleColor.White;
    }
    private const string _defaultTexture = """
              __________
             /          \
            /            \
           /              \
          /________________\
          |                |
          |                |
          |   [__]   [__]  |
          |                |
          |                |
          |       ___      |
          |      |   |     |
          |______|___|_____|
          """;
}
