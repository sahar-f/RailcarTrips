using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailcarTrips.Data;
using RailcarTrips.Shared.Models;

namespace RailcarTrips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController(RailcarTripsContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Trip>>> GetTrips()
    {
        var trips = await context.Trips
            .Include(t => t.OriginCity)
            .Include(t => t.DestinationCity)
            .ToListAsync();

        return Ok(trips);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Trip>> GetTripById(int id)
    {
        var trip = await context.Trips
            .Include(t => t.OriginCity)
            .Include(t => t.DestinationCity)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (trip == null)
            return NotFound();

        return Ok(trip);
    }

    [HttpGet("{id}/events")]
    public async Task<ActionResult<IEnumerable<EquipmentEvent>>> GetTripEvents(int id)
    {
        var trip = await context.Trips.FindAsync(id);
        if (trip == null)
            return NotFound();
        
        var events = await context.EquipmentEvents
            .Where(e => e.EquipmentId == trip.EquipmentId)
            .OrderBy(e => e.EventTime)
            .Include(e => e.City)
            .ToListAsync();
        
        return Ok(events);
    }
}