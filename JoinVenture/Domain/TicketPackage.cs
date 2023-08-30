using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain
{
    public class TicketPackage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        [JsonIgnore]
        public Activity Activity { get; set; }
        public  DateTime ValidatedDateStart { get; set; }
        public DateTime ValidatedDateEnd { get; set; }
        public DateTime BookingAvailableStart { get; set; }
        public DateTime BookingAvailableEnd { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}