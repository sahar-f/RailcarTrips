using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailcarTrips.Data;
using RailcarTrips.Shared.Models;

namespace RailcarTrips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitiesController(RailcarTripsContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
        return await context.Cities.ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> AddCity(City city)
    {
        context.Cities.Add(city);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCities), new { id = city.Id }, city);
    }
}