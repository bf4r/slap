namespace slap.Things;
using slap.Things.Society;

public class Location
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Location(string? name = null, string? description = null, double latitude = 0, double longitude = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
    }
    public double DistanceToInMeters(Location location2)
    {
        var earthRadius = 6371;
        var lat1 = ToRadian(this.Latitude);
        var lat2 = ToRadian(location2.Latitude);
        var dLat = ToRadian(location2.Latitude - this.Latitude);
        var dLon = ToRadian(location2.Longitude - this.Longitude);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadius * c * 1000.0;
    }
    private double ToRadian(double degrees)
    {
        return degrees * Math.PI / 180;
    }
    public static Location Get(CommonLocations.CommonCities city)
    {
        if (CommonLocations.CityMap.ContainsKey(city)) return CommonLocations.CityMap[city];

        throw new NotImplementedException($"The location {city.ToString()} is in the CommonCities list but is not added to CityMap.");
    }
}
