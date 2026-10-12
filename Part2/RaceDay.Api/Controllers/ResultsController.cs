using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
namespace RaceDay.Api.Controllers;
[ApiController, Route("api")]
public class ResultsController(RaceDayDbContext db) : ControllerBase
{
    [HttpPost("enrolments/{enrolmentId:int}/results"), Authorize(Roles="Organiser")]
    public async Task<IActionResult> Create(int enrolmentId, Result input)
    {
        var e=await db.Enrolments.Include(x=>x.Category).ThenInclude(x=>x!.Event).SingleOrDefaultAsync(x=>x.EnrolmentID==enrolmentId);if(e is null)return NotFound();
        if(e.Category?.Event?.OrganiserID!=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))return Forbid();
        if(await db.Results.AnyAsync(x=>x.EnrolmentID==enrolmentId))return Conflict(new{message="Result exists; use PUT."});
        input.ResultID=0;input.EnrolmentID=enrolmentId;input.Enrolment=null;db.Results.Add(input);await db.SaveChangesAsync();return Created($"/api/results/{input.ResultID}",input);
    }
    [HttpGet("results/me"), Authorize(Roles="Participant")]
    public async Task<IActionResult> Mine(){var id=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);return Ok(await db.Results.AsNoTracking().Include(x=>x.Enrolment).ThenInclude(x=>x!.Category).ThenInclude(x=>x!.Event).Where(x=>x.Enrolment!.ParticipantID==id).ToListAsync());}
    [HttpGet("events/{eventId:int}/results"), Authorize(Roles="Organiser")]
    public async Task<IActionResult> ForEvent(int eventId)
    {
        var ev=await db.Events.FindAsync(eventId);if(ev is null)return NotFound();if(ev.OrganiserID!=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))return Forbid();
        return Ok(await db.Results.AsNoTracking().Include(x=>x.Enrolment).Where(x=>x.Enrolment!.Category!.EventID==eventId).ToListAsync());
    }
    [HttpGet("results/{id:int}"), Authorize]
    public async Task<IActionResult> Get(int id)
    {
        var r=await db.Results.AsNoTracking().Include(x=>x.Enrolment).ThenInclude(x=>x!.Category).ThenInclude(x=>x!.Event).SingleOrDefaultAsync(x=>x.ResultID==id);if(r is null)return NotFound();
        var uid=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);var role=User.FindFirstValue(ClaimTypes.Role);
        if(role=="Participant"&&r.Enrolment?.ParticipantID!=uid)return Forbid();if(role=="Organiser"&&r.Enrolment?.Category?.Event?.OrganiserID!=uid)return Forbid();return Ok(r);
    }
    [HttpPut("results/{id:int}"), Authorize(Roles="Organiser")]
    public async Task<IActionResult> Update(int id, Result input)
    {
        var r=await db.Results.Include(x=>x.Enrolment).ThenInclude(x=>x!.Category).ThenInclude(x=>x!.Event).SingleOrDefaultAsync(x=>x.ResultID==id);if(r is null)return NotFound();
        if(r.Enrolment?.Category?.Event?.OrganiserID!=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))return Forbid();
        r.FinishTime=input.FinishTime;r.Position=input.Position;r.ResultStatus=input.ResultStatus;await db.SaveChangesAsync();return Ok(r);
    }
    [HttpDelete("results/{id:int}"), Authorize(Roles="Organiser")]
    public async Task<IActionResult> Delete(int id)
    {
        var r=await db.Results.Include(x=>x.Enrolment).ThenInclude(x=>x!.Category).ThenInclude(x=>x!.Event).SingleOrDefaultAsync(x=>x.ResultID==id);if(r is null)return NotFound();
        if(r.Enrolment?.Category?.Event?.OrganiserID!=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))return Forbid();db.Results.Remove(r);await db.SaveChangesAsync();return NoContent();
    }
}
