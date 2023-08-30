
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Activity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public string Image { get; set; }
        public int Followers { get; set; }
        public int Tickets { get; set; }
        public bool IsCancelled { get; set; }

        public ICollection<ActivityAttendee> Attendees { get; set; } = new List<ActivityAttendee>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<TicketPackage> TicketPackages { get; set; } = new List<TicketPackage>();
    }
}