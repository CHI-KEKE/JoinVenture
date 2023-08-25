using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
    public class Create
    {
        public class Command : IRequest<Guid>
        {
            public Order Order{get;set;}
        }

        public class Handler : IRequestHandler<Command,Guid>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context,IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
                
            }
            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                
                user.Orders.Add(request.Order);

                await _context.SaveChangesAsync();


                var newestOrderId = user.Orders
                    .OrderByDescending(o => o.InvoiceDate) // Assuming InvoiceDate is a DateTime property
                    .Select(o => o.Id)
                    .FirstOrDefault();

                return newestOrderId; // Return the ID of the newest order
            }
        }        
    }
}