namespace RailcarTrips.Shared.Models;

public class EquipmentEvent
{
    public int Id { get; set; }
    public string EquipmentId { get; set; } = string.Empty;
    public int CityId { get; set; }
    public char EventCode { get; set; }
    public DateTime EventTime { get; set; }

    public City? City { get; set; }
}