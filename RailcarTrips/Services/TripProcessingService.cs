using Microsoft.EntityFrameworkCore;
using RailcarTrips.Data;
using RailcarTrips.Services.Interfaces;
using RailcarTrips.Shared.Models;

namespace RailcarTrips.Services;

public class TripProcessingService(RailcarTripsContext context) : ITripProcessingService
{
    public async Task ProcessTripsAsync()
    {
        var equipmentIds = await context.EquipmentEvents
            .Select(e => e.EquipmentId)
            .Distinct()
            .ToListAsync();

        foreach (var equipmentId in equipmentIds)
        {
            var events = await GetEventsForEquipmentAsync(equipmentId);
            var trips = CreateTripsFromEvents(events);
            await SaveTripsAsync(trips);
        }
    }

    private async Task<List<EquipmentEvent>> GetEventsForEquipmentAsync(string equipmentId) =>
        await context.EquipmentEvents
            .Where(e => e.EquipmentId == equipmentId)
            .OrderBy(e => e.EventTime)
            .ToListAsync();

    private static List<Trip> CreateTripsFromEvents(List<EquipmentEvent> events)
    {
        List<Trip> trips = [];
        Trip? currentTrip = null;

        foreach (var evnt in events)
        {
            if (evnt.EventCode == 'W') 
            {
                currentTrip = new Trip
                {
                    EquipmentId = evnt.EquipmentId,
                    OriginCityId = evnt.CityId,
                    StartUtc = evnt.EventTime
                };
            }
            else if (evnt.EventCode == 'Z' && currentTrip != null) 
            {
                currentTrip.DestinationCityId = evnt.CityId;
                currentTrip.EndUtc = evnt.EventTime;
                currentTrip.TotalTripHours = (float)(evnt.EventTime - currentTrip.StartUtc).TotalHours;

                trips.Add(currentTrip);
                currentTrip = null; // Reset for next trip
            }
        }

        return trips;
    }
    
    private async Task SaveTripsAsync(List<Trip> trips)
    {
        if (trips.Count != 0)
        {
            context.Trips.AddRange(trips);
            await context.SaveChangesAsync();
        }
    }
}