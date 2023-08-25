using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser:IdentityUser
    {
        public string ShowName { get; set; }
        public string Bio { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public ICollection<ActivityAttendee> Activities { get; set; }
        public ICollection<Photo> Photos { get; set; } = new List<Photo>(); 
        public ICollection<Order> Orders { get; set; } = new List<Order>(); 
   }
}