using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
namespace RaceDay.Api.Controllers;
public record ProfileUpdate(string FirstName, string LastName, string Email);
[ApiController, Route("api/users"), Authorize]
public class UsersController(RaceDayDbContext db) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var u = await db.Users.FindAsync(id);
        return u is null ? NotFound() : Ok(new { u.UserID, u.FirstName, u.LastName, u.Email, u.Role });
    }
    [HttpPut("me")]
    public async Task<IActionResult> Update(ProfileUpdate r)
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var u = await db.Users.FindAsync(id);
        if (u is null) return NotFound();
        var email = r.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(x => x.Email == email && x.UserID != id)) return Conflict(new { message = "Email already in use." });
        u.FirstName = r.FirstName.Trim(); u.LastName = r.LastName.Trim(); u.Email = email;
        await db.SaveChangesAsync();
        return Ok(new { u.UserID, u.FirstName, u.LastName, u.Email, u.Role });
    }
    [HttpGet("{id:int}"), Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Get(int id)
    {
        var u = await db.Users.FindAsync(id);
        return u is null ? NotFound() : Ok(new { u.UserID, u.FirstName, u.LastName, u.Email, u.Role });
    }
}
