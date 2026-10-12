using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
namespace RaceDay.Api.Controllers;
[ApiController, Route("api")]
public class CategoriesController(RaceDayDbContext db) : ControllerBase
{
    [HttpGet("events/{eventId:int}/categories"), AllowAnonymous]
    public async Task<IActionResult> ForEvent(int eventId) => !await db.Events.AnyAsync(x=>x.EventID==eventId) ? NotFound() : Ok(await db.Categories.AsNoTracking().Where(x=>x.EventID==eventId).ToListAsync());
    [HttpGet("categories/{id:int}"), AllowAnonymous]
    public async Task<IActionResult> Get(int id) { var c=await db.Categories.FindAsync(id); return c is null ? NotFound() : Ok(c); }
    [HttpPost("events/{eventId:int}/categories"), Authorize(Roles="Organiser")]
    public async Task<IActionResult> Create(int eventId, Category input)
    {
        var e=await db.Events.FindAsync(eventId); if(e is null) return NotFound();
        if(e.OrganiserID!=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)) return Forbid();
        input.CategoryID=0; input.EventID=eventId; input.Event=null; db.Categories.Add(input); await db.SaveChangesAsync(); return Created($"/api/categories/{input.CategoryID}",input);
    }
    [HttpPut("categories/{id:int}"), Authorize(Roles="Organiser")]
    public async Task<IActionResult> Update(int id, Category input)
    {
        var c=await db.Categories.Include(x=>x.Event).SingleOrDefaultAsync(x=>x.CategoryID==id); if(c is null) return NotFound();
        if(c.Event?.OrganiserID!=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)) return Forbid();
        c.CategoryName=input.CategoryName; c.EntryFee=input.EntryFee; c.MaxParticipants=input.MaxParticipants; await db.SaveChangesAsync(); return Ok(c);
    }
    [HttpDelete("categories/{id:int}"), Authorize(Roles="Organiser")]
    public async Task<IActionResult> Delete(int id)
    {
        var c=await db.Categories.Include(x=>x.Event).SingleOrDefaultAsync(x=>x.CategoryID==id); if(c is null) return NotFound();
        if(c.Event?.OrganiserID!=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)) return Forbid();
        db.Categories.Remove(c); await db.SaveChangesAsync(); return NoContent();
    }
}
