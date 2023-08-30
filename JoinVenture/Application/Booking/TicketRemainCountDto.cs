using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Tickets
{
    public class TicketRemainCountDto
    {
        public string PackageTitle { get; set; }
        public int Count { get; set; }        
    }
}