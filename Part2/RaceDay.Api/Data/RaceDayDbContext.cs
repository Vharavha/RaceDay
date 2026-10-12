using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Models;
namespace RaceDay.Api.Data;
public class RaceDayDbContext(DbContextOptions<RaceDayDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Enrolment> Enrolments => Set<Enrolment>();
    public DbSet<Result> Results => Set<Result>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>().HasIndex(x => x.Email).IsUnique();
        b.Entity<Event>().HasOne(x => x.Organiser).WithMany(x => x.OrganisedEvents).HasForeignKey(x => x.OrganiserID).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Category>().HasIndex(x => new { x.EventID, x.CategoryName }).IsUnique();
        b.Entity<Category>().HasOne(x => x.Event).WithMany(x => x.Categories).HasForeignKey(x => x.EventID).OnDelete(DeleteBehavior.Cascade);
        b.Entity<Enrolment>().HasIndex(x => new { x.ParticipantID, x.CategoryID }).IsUnique();
        b.Entity<Enrolment>().HasOne(x => x.Participant).WithMany(x => x.Enrolments).HasForeignKey(x => x.ParticipantID).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Enrolment>().HasOne(x => x.Category).WithMany(x => x.Enrolments).HasForeignKey(x => x.CategoryID).OnDelete(DeleteBehavior.Cascade);
        b.Entity<Result>().HasIndex(x => x.EnrolmentID).IsUnique();
        b.Entity<Result>().HasOne(x => x.Enrolment).WithOne(x => x.Result).HasForeignKey<Result>(x => x.EnrolmentID).OnDelete(DeleteBehavior.Cascade);
    }
}
