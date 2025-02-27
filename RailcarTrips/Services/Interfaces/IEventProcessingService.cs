using RailcarTrips.Shared.Models;

namespace RailcarTrips.Services.Interfaces;

public interface IEventProcessingService
{
    Task<List<EquipmentEvent>> GetEquipmentEventsAsync();
    Task ProcessEquipmentEventsAsync(IFormFile file);
}