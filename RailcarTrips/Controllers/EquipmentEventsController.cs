using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailcarTrips.Data;
using RailcarTrips.Shared.Models;

namespace RailcarTrips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EquipmentEventsController(RailcarTripsContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipmentEvent>>> GetEquipmentEvents()
    {
        return await context.EquipmentEvents.Include(e => e.City).ToListAsync();
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("Invalid file.");
        
        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<EquipmentEvent>().ToList();

            context.EquipmentEvents.AddRange(records);
            await context.SaveChangesAsync();

            return Ok("File uploaded and processed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error processing file: {ex.Message}");
        }
    }
}