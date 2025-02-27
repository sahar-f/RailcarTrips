namespace RailcarTrips.Models;

public class Trip
{
    public int Id { get; set; }
    public string EquipmentId { get; set; } = string.Empty;
    public int OriginCityId { get; set; }
    public int DestinationCityId { get; set; }
    public DateTime StartUtc { get; set; }
    public DateTime? EndUtc { get; set; }
    public float? TotalTripHours { get; set; }

    public City? OriginCity { get; set; }
    public City? DestinationCity { get; set; }
}