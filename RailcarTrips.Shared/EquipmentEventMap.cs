using CsvHelper.Configuration;
using RailcarTrips.Shared.Models;

namespace RailcarTrips.Shared;

public sealed class EquipmentEventMap : ClassMap<EquipmentEvent>
{
    public EquipmentEventMap()
    {
        Map(m => m.EquipmentId).Name("Equipment Id", "EquipmentId");
        Map(m => m.EventCode).Name("Event Code", "EventCode");
        Map(m => m.EventTime).Name("Event Time", "EventTime");
        Map(m => m.CityId).Name("City Id", "CityId");

        // Ignore the navigation property
        Map(m => m.City).Ignore();
    }
}