using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Persistence;

namespace Application.Core
{
    public class TicketIdToTicketConverter : ITypeConverter<string, Ticket>
    {

        private readonly DataContext _dbContext; // Inject your DbContext here

        public TicketIdToTicketConverter(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Ticket Convert(string source, Ticket destination, ResolutionContext context)
        {
            if (Guid.TryParse(source, out var ticketId))
            {
                var ticket = _dbContext.Tickets.FirstOrDefault(t => t.Id == ticketId);
                return ticket;
            }

            return null; 
        }
    }
}