using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public string? UserId { get; set; }
        public int? Version { get; set; }
        
        [JsonIgnore]
        public TicketPackage TicketPackage { get; set; }
    }
}