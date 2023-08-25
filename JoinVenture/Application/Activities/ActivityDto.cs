using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Profiles;
using Domain;

namespace Application.Activities
{
    public class ActivityDto
    {
         public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public string Host { get; set; }
        public string Image { get; set; }
        public int Followers { get; set; }
        public int Tickets { get; set; }      

        public string HostUserName { get; set; } 
        public bool IsCancelled { get; set; }

        public ICollection<Profile> Attendees  { get; set; }
        public ICollection<TicketPackage> TicketPackages{get;set;}
    }
}