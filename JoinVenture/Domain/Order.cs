using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public string ActivityTitle { get; set; }
        public string ActivityImage { get; set; }
        public int ActivityId { get; set; }
        public List<BookedTicketPackage> BookedTicketPackages { get; set; }  = new List<BookedTicketPackage>();
        public int TotalPrice { get; set; }
        public bool Discount { get; set; }
        public DateTime InvoiceDate { get; set; }

        public string AppUserId  { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
    
}