using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain
{
    public class BookedTicketPackage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public Guid ActivityId { get; set; }
        public string ValidDate { get; set; }
        
        [JsonIgnore]
        public Order Order { get; set; }   
    }
}