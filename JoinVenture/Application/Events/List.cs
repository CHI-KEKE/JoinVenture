using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using Application.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Events
{
    public class List
    {
        public class Query : IRequest<List<ActivityDto>> {}

        public class Handler : IRequestHandler<Query, List<ActivityDto>>
        {
            private readonly ILogger<List> _logger;
            private readonly IMapper _mapper;

            private readonly DataContext _context;
            public Handler(DataContext context,ILogger<List> logger,IMapper mapper)
            {
                _mapper = mapper;
                _logger = logger;
                _context = context;
                
            }
            public async Task<List<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {

                var activities = await _context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

                // var activitiesToReturn = _mapper.Map<List<ActivityDto>>(activities);

                return activities;
            }
        }
    }
}