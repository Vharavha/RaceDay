using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RaceDay.Api.Data;
using Xunit;
namespace RaceDay.Tests;
public class RaceDayApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<RaceDayDbContext>>();
            services.RemoveAll<RaceDayDbContext>();
            services.AddDbContext<RaceDayDbContext>(o => o.UseInMemoryDatabase("RaceDayTestDb"));
        });
    }
}
public class ApiSmokeTests : IClassFixture<RaceDayApiFactory>
{
    private readonly RaceDayApiFactory factory;
    public ApiSmokeTests(RaceDayApiFactory factory) => this.factory = factory;
    [Fact]
    public async Task PublicEvents_AreAccessible() { using var c=factory.CreateClient(); Assert.Equal(HttpStatusCode.OK,(await c.GetAsync("/api/events")).StatusCode); }
    [Fact]
    public async Task ParticipantRoute_RejectsUnauthenticatedRequest() { using var c=factory.CreateClient(); Assert.Equal(HttpStatusCode.Unauthorized,(await c.GetAsync("/api/enrolments/me")).StatusCode); }
    [Fact]
    public async Task Registration_RejectsInvalidRole()
    {
        using var c=factory.CreateClient();
        var response=await c.PostAsJsonAsync("/api/auth/register",new{firstName="Test",lastName="User",email="badrole@example.com",password="ExamplePassword!234",role="Admin"});
        Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
    }
    [Fact]
    public async Task Registration_DoesNotReturnPassword()
    {
        using var c=factory.CreateClient();
        var response=await c.PostAsJsonAsync("/api/auth/register",new{firstName="Test",lastName="User",email=Guid.NewGuid()+"@example.com",password="ExamplePassword!234",role="Participant"});
        Assert.Equal(HttpStatusCode.Created,response.StatusCode);
        var body=await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("PasswordHash",body,StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ExamplePassword!234",body);
    }
}
