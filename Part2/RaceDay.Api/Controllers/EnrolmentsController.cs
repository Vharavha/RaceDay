using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
namespace RaceDay.Api.Controllers;
[ApiController, Route("api")]
public class EnrolmentsController(RaceDayDbContext db) : ControllerBase
{
    [HttpPost("categories/{categoryId:int}/enrolments"), Authorize(Roles="Participant")]
    public async Task<IActionResult> Enrol(int categoryId)
    {
        var pid=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); var c=await db.Categories.FindAsync(categoryId); if(c is null)return NotFound();
        if(await db.Enrolments.AnyAsync(x=>x.ParticipantID==pid&&x.CategoryID==categoryId))return Conflict(new{message="Already enrolled."});
        if(c.MaxParticipants.HasValue && await db.Enrolments.CountAsync(x=>x.CategoryID==categoryId&&x.Status!="Cancelled")>=c.MaxParticipants.Value)return Conflict(new{message="Category is full."});
        var e=new Enrolment{ParticipantID=pid,CategoryID=categoryId}; db.Enrolments.Add(e); await db.SaveChangesAsync(); return Created($"/api/enrolments/{e.EnrolmentID}",e);
    }
    [HttpGet("enrolments/me"), Authorize(Roles="Participant")]
    public async Task<IActionResult> Mine(){var id=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);return Ok(await db.Enrolments.AsNoTracking().Include(x=>x.Category).ThenInclude(x=>x!.Event).Where(x=>x.ParticipantID==id).ToListAsync());}
    [HttpGet("events/{eventId:int}/enrolments"), Authorize(Roles="Organiser")]
    public async Task<IActionResult> ForEvent(int eventId)
    {
        var e=await db.Events.FindAsync(eventId); if(e is null)return NotFound(); if(e.OrganiserID!=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))return Forbid();
        return Ok(await db.Enrolments.AsNoTracking().Include(x=>x.Participant).Include(x=>x.Category).Where(x=>x.Category!.EventID==eventId).ToListAsync());
    }
    [HttpGet("enrolments/{id:int}"), Authorize]
    public async Task<IActionResult> Get(int id)
    {
        var e=await db.Enrolments.AsNoTracking().Include(x=>x.Category).ThenInclude(x=>x!.Event).SingleOrDefaultAsync(x=>x.EnrolmentID==id); if(e is null)return NotFound();
        var uid=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); var role=User.FindFirstValue(ClaimTypes.Role);
        if(role=="Participant"&&e.ParticipantID!=uid)return Forbid(); if(role=="Organiser"&&e.Category?.Event?.OrganiserID!=uid)return Forbid(); return Ok(e);
    }
    [HttpDelete("enrolments/{id:int}"), Authorize(Roles="Participant")]
    public async Task<IActionResult> Cancel(int id){var e=await db.Enrolments.FindAsync(id);if(e is null)return NotFound();if(e.ParticipantID!=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))return Forbid();e.Status="Cancelled";await db.SaveChangesAsync();return NoContent();}
}
