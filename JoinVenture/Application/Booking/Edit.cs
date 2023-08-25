using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;
using AutoMapper;
using Application.Interface;
using Microsoft.EntityFrameworkCore;

namespace Application.Booking
{
    public class Edit
    {
        public class Command:IRequest<Ticket>
        {
            public Ticket Ticket {get;set;}
        }

        public class Handler : IRequestHandler<Command,Ticket>
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
            public async Task<Ticket> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                Console.WriteLine(user + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!user here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                request.Ticket.UserId = user.Id;


                // activity.Title = request.Activity.Title ?? activity.Title;

                // _mapper.Map(request.Activity,activity);


                await _context.SaveChangesAsync();

                return request.Ticket;
            }
        }
    }
}