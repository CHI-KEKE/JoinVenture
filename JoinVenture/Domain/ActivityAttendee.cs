using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain
{
    public class ActivityAttendee
    {
        public string AppUserId { get; set; }
        [JsonIgnore]
        public AppUser AppUser { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

        public bool IsHost { get; set; }
    }
}