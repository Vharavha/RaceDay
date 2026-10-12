using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
namespace RaceDay.Api.Controllers;
[ApiController, Route("api/events")]
public class EventsController(RaceDayDbContext db) : ControllerBase
{
    [HttpGet, AllowAnonymous] public async Task<IActionResult> All() => Ok(await db.Events.AsNoTracking().Include(x => x.Categories).ToListAsync());
    [HttpGet("{id:int}"), AllowAnonymous] public async Task<IActionResult> Get(int id)
    {
        var e = await db.Events.AsNoTracking().Include(x => x.Categories).SingleOrDefaultAsync(x => x.EventID == id);
        return e is null ? NotFound() : Ok(e);
    }
    [HttpPost, Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Create(Event e)
    {
        if (e.EventType is not ("Run" or "Walk" or "Cycle")) return BadRequest(new { message = "EventType must be Run, Walk, or Cycle." });
        e.EventID = 0; e.OrganiserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); e.Organiser = null;
        db.Events.Add(e); await db.SaveChangesAsync(); return CreatedAtAction(nameof(Get), new { id = e.EventID }, e);
    }
    [HttpPut("{id:int}"), Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Update(int id, Event input)
    {
        var e = await db.Events.FindAsync(id); if (e is null) return NotFound();
        if (e.OrganiserID != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)) return Forbid();
        if (input.EventType is not ("Run" or "Walk" or "Cycle")) return BadRequest(new { message = "EventType must be Run, Walk, or Cycle." });
        e.EventName=input.EventName; e.Description=input.Description; e.EventDate=input.EventDate; e.Location=input.Location; e.DistanceKM=input.DistanceKM; e.EventType=input.EventType; e.Status=input.Status;
        await db.SaveChangesAsync(); return Ok(e);
    }
    [HttpDelete("{id:int}"), Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await db.Events.FindAsync(id); if (e is null) return NotFound();
        if (e.OrganiserID != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)) return Forbid();
        db.Events.Remove(e); await db.SaveChangesAsync(); return NoContent();
    }
}
