using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Tickets;
using Application.Activities;
using Application.Core;
using Application.Events;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Booking
{
    public class Booking
    {
        public class Command:IRequest<Result<int>>
        {
            public int ActivityId{get;set;}
        }


        public class Handler : IRequestHandler<Command, Result<int>>
        {
            private readonly IMapper _mapper;
            private readonly DataContext _context;
            public Handler(DataContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activityinfo = await _context.Activities
                .FirstOrDefaultAsync(x => x.Id == request.ActivityId);

                if (activityinfo.Tickets != null && activityinfo.Tickets > 0)
                {
                    activityinfo.Tickets--; // Reduce the ticket count
                    await _context.SaveChangesAsync();

                    int updatedTicketCount = activityinfo.Tickets;
                    
                    return Result<int>.Success(updatedTicketCount);
                }

                return Result<int>.Failure("Tickets sold out");
            }

        }        


    }
}