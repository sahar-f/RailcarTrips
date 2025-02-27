using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using RailcarTrips.Data;
using RailcarTrips.Services.Interfaces;
using RailcarTrips.Shared;
using RailcarTrips.Shared.Models;

namespace RailcarTrips.Services;

public class EventProcessingService(RailcarTripsContext context, ITripProcessingService tripProcessingService)
    : IEventProcessingService
{
    public async Task<List<EquipmentEvent>> GetEquipmentEventsAsync()
    {
        try
        {
            // not going to be ordered data - that's fine.
            return await context.EquipmentEvents
                .Include(e => e.City)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error in GetEquipmentEventsAsync: {ex.Message}");
            throw;
        }
    }

    public async Task ProcessEquipmentEventsAsync(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<EquipmentEventMap>();
        var records = csv.GetRecords<EquipmentEvent>().ToList();

        foreach (var record in records)
        {
            record.City = await context.Cities.FindAsync(record.CityId);
            if (record.City == null) continue;
            record.EventTime = ConvertToUtc(record.EventTime, record.City.TimeZone);
        }

        context.EquipmentEvents.AddRange(records);
        await context.SaveChangesAsync();

        // Process trips now so both get updated - have to figure out page refreshing.
        await tripProcessingService.ProcessTripsAsync();
    }

    private static DateTime ConvertToUtc(DateTime localTime, string timeZone)
    {
        try
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            //Handle invalid time that would be skipped due to DST by adding a minute
            return TimeZoneInfo.ConvertTimeToUtc(!timeZoneInfo.IsInvalidTime(localTime) ? localTime : localTime.AddMinutes(1), timeZoneInfo);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting time: {ex.Message}");
            return localTime; //Worse caase- have to check
        }
    }
}