using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class OrderDto
    {
        public string ActivityTitle { get; set; }
        public string ActivityImage { get; set; }
        public int ActivityId { get; set; }
        public List<BookedTicketPackageDto> BookedTicketPackages { get; set; }
        public int TotalPrice { get; set; }
        public bool Discount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public List<string> Tickets { get; set; }
    }

    public class BookedTicketPackageDto
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int ActivityId { get; set; }
        public string ValidDate { get; set; }
    }
}