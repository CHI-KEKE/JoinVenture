using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Events
{
    public class Details
    {
        public class Query : IRequest<ActivityDto>
        {
            public Guid Id {get;set;}
        }


        public class Handler : IRequestHandler<Query, ActivityDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context,IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }
            public async Task<ActivityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Activities.Include(a => a.TicketPackages).ThenInclude(tp =>tp.Tickets)
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            }
        }
    }
}