using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Interface;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
    public class Detail
    {
         public class Query : IRequest<Order>
        {
            public Guid ActivityId {get;set;}
        }


        public class Handler : IRequestHandler<Query, Order>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context,IMapper mapper,IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }
            public async Task<Order> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                Console.WriteLine(user+ "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!user here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                var selectedOrder = await _context.Orders
                .SingleOrDefaultAsync(o => o.ActivityId == request.ActivityId && o.AppUserId == user.Id);
                
                var orderJson = JsonSerializer.Serialize(selectedOrder);
                Console.WriteLine(orderJson + "Found Order!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");


                return await _context.Orders
                .SingleOrDefaultAsync(o => o.ActivityId == request.ActivityId && o.AppUserId == user.Id);

            }
        }       
    }
}