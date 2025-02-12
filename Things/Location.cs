namespace slap.Things;

public class Location
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public Location(string? name = null, string? description = null, int x = 0, int y = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        X = x;
        Y = y;
    }
    public Location(int x, int y) : this(null, null, x, y) { }
    public double DistanceTo(Location location2)
    {
        int dX = location2.X - this.X;
        int dY = location2.Y - this.Y;
        return Math.Sqrt(dX * dX + dY * dY);
    }
    public static Location GetRandomLocation()
    {
        return new Location(
                    name: null,
                    description: null,
                    x: Sim.Random.Next(-1000, 1000),
                    y: Sim.Random.Next(-1000, 1000)
                );
    }
}
