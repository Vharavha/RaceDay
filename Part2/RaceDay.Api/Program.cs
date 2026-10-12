using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RaceDayDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o => { o.Cookie.Name = ".RaceDay.Session"; o.Cookie.HttpOnly = true; o.Cookie.IsEssential = true; o.IdleTimeout = TimeSpan.FromMinutes(30); });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o =>
{
    o.Cookie.Name = ".RaceDay.Auth"; o.Cookie.HttpOnly = true; o.Cookie.IsEssential = true; o.SlidingExpiration = true; o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    o.Events.OnRedirectToLogin = c => { c.Response.StatusCode = 401; return Task.CompletedTask; };
    o.Events.OnRedirectToAccessDenied = c => { c.Response.StatusCode = 403; return Task.CompletedTask; };
});
builder.Services.AddAuthorization();
var app = builder.Build();
app.UseSwagger(); app.UseSwaggerUI();
app.UseHttpsRedirection(); app.UseSession(); app.UseAuthentication(); app.UseAuthorization(); app.MapControllers();
app.Run();
public partial class Program { }
