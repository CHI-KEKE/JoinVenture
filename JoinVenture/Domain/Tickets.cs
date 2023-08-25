using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public string? UserId { get; set; }
        
        [JsonIgnore]
        public TicketPackage TicketPackage { get; set; }
    }
}