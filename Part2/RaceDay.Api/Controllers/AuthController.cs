using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
namespace RaceDay.Api.Controllers;
public record RegisterRequest(string FirstName, string LastName, string Email, string Password, string Role);
public record LoginRequest(string Email, string Password);
public record UserResponse(int UserID, string FirstName, string LastName, string Email, string Role);
[ApiController, Route("api/auth")]
public class AuthController(RaceDayDbContext db) : ControllerBase
{
    private readonly PasswordHasher<User> hasher = new();
    /// <summary>Register an Organiser or Participant account. Password is stored as a hash.</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest r)
    {
        if (string.IsNullOrWhiteSpace(r.FirstName) || string.IsNullOrWhiteSpace(r.LastName) || !r.Email.Contains('@') || r.Password.Length < 8 || r.Role is not ("Organiser" or "Participant"))
            return BadRequest(new { message = "Provide names, a valid email, password of at least 8 characters, and role Organiser or Participant." });
        var email = r.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(u => u.Email == email)) return Conflict(new { message = "Email already registered." });
        var u = new User { FirstName = r.FirstName.Trim(), LastName = r.LastName.Trim(), Email = email, Role = r.Role };
        u.PasswordHash = hasher.HashPassword(u, r.Password);
        db.Users.Add(u); await db.SaveChangesAsync();
        return Created($"/api/users/{u.UserID}", new UserResponse(u.UserID, u.FirstName, u.LastName, u.Email, u.Role));
    }
    /// <summary>Login; issues an HTTP-only cookie and records the user ID in session.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest r)
    {
        var u = await db.Users.SingleOrDefaultAsync(x => x.Email == r.Email.Trim().ToLowerInvariant());
        if (u is null || hasher.VerifyHashedPassword(u, u.PasswordHash, r.Password) == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Invalid email or password." });
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, u.UserID.ToString()), new Claim(ClaimTypes.Name, u.Email), new Claim(ClaimTypes.Role, u.Role) };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
        HttpContext.Session.SetInt32("UserID", u.UserID);
        return Ok(new UserResponse(u.UserID, u.FirstName, u.LastName, u.Email, u.Role));
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear(); await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Logged out." });
    }
}
