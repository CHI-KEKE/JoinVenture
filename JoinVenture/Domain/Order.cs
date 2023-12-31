using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public string ActivityTitle { get; set; }
        public string ActivityImage { get; set; }
        public Guid ActivityId { get; set; }
        public List<BookedTicketPackage> BookedTicketPackages { get; set; }  = new List<BookedTicketPackage>();
        public int TotalPrice { get; set; }
        public bool Discount { get; set; }
        public DateTime InvoiceDate { get; set; }

        public string AppUserId  { get; set; }
        public string TicketId { get; set; }
    }
    
}