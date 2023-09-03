using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace Application.Activities
{
    public class ActivityCreateRequestDto
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public IFormFile Image { get; set; }
        public List<TicketPackageDTO> TicketPackages { get; set; }        
    }

    public class TicketPackageDTO
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public string ValidatedDateStart { get; set; }
        public string ValidatedDateEnd { get; set; }
        public string BookingAvailableStart { get; set; }
        public string BookingAvailableEnd { get; set; }
    }
}