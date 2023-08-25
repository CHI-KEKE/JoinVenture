using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Tickets
{
    public class TicketCheckingDto
    {
        public string PackageTitle { get; set; }
        public int Quantity { get; set; }
    }
}