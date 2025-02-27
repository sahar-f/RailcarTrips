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
}