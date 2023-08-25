using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Tickets
{
    public class AvailibleTicketsDto
    {
        public string PackageTitle { get; set; }
        public Guid TicketId { get; set; }
        public string Stauts { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public string UserId { get; set; }
    }
}