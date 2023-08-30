using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public AppUser Author { get; set; }
        [JsonIgnore]
        public Activity Activity { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}