using System.ComponentModel.DataAnnotations;
namespace RaceDay.Api.Models;
public class User
{
    public int UserID { get; set; }
    [Required, MaxLength(50)] public string FirstName { get; set; } = "";
    [Required, MaxLength(50)] public string LastName { get; set; } = "";
    [Required, EmailAddress, MaxLength(100)] public string Email { get; set; } = "";
    [Required] public string PasswordHash { get; set; } = "";
    [Required, RegularExpression("^(Organiser|Participant)$")] public string Role { get; set; } = "Participant";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Event> OrganisedEvents { get; set; } = new List<Event>();
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
}
public class Event
{
    public int EventID { get; set; }
    [Required] public int OrganiserID { get; set; }
    [Required, MaxLength(100)] public string EventName { get; set; } = "";
    [MaxLength(500)] public string? Description { get; set; }
    public DateOnly EventDate { get; set; }
    [Required, MaxLength(150)] public string Location { get; set; } = "";
    [Range(0.01, 9999)] public decimal DistanceKM { get; set; }
    [Required, RegularExpression("^(Run|Walk|Cycle)$")] public string EventType { get; set; } = "Run";
    public string Status { get; set; } = "Upcoming";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User? Organiser { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
public class Category
{
    public int CategoryID { get; set; }
    public int EventID { get; set; }
    [Required, MaxLength(100)] public string CategoryName { get; set; } = "";
    [Range(0, 100000)] public decimal EntryFee { get; set; }
    [Range(1, 1000000)] public int? MaxParticipants { get; set; }
    public Event? Event { get; set; }
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
}
public class Enrolment
{
    public int EnrolmentID { get; set; }
    public int ParticipantID { get; set; }
    public int CategoryID { get; set; }
    public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Confirmed";
    public User? Participant { get; set; }
    public Category? Category { get; set; }
    public Result? Result { get; set; }
}
public class Result
{
    public int ResultID { get; set; }
    public int EnrolmentID { get; set; }
    public TimeSpan? FinishTime { get; set; }
    [Range(1, int.MaxValue)] public int? Position { get; set; }
    public string ResultStatus { get; set; } = "Finished";
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    public Enrolment? Enrolment { get; set; }
}
