using Microsoft.AspNetCore.Mvc;
using RailcarTrips.Services.Interfaces;
using RailcarTrips.Shared.Models;

namespace RailcarTrips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EquipmentEventsController(IEventProcessingService eventProcessingService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipmentEvent>>> GetEquipmentEvents()
    {
        try
        {
            var events = await eventProcessingService.GetEquipmentEventsAsync();
            return Ok(events);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetEquipmentEvents: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving equipment events.");
        }
    }

    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("Invalid file.");
        
        try
        {
            try
            {
                await eventProcessingService.ProcessEquipmentEventsAsync(file);
                return Ok("File uploaded and processed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error processing file: {ex.Message}");
        }
    }
}