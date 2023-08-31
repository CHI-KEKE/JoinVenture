using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
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
    public class ListHosted
    {
        public class Query : IRequest<Result<List<Activity>>>
         {
            public string AccessToken{get;set;}
         }

        public class Handler : IRequestHandler<Query, Result<List<Activity>>>
        {
            private readonly ILogger<List> _logger;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            private readonly DataContext _context;
            public Handler(DataContext context,ILogger<List> logger,IMapper mapper,IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _logger = logger;
                _context = context;
                
            }
            public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
            {

                Console.WriteLine(_userAccessor.GetUsername());
                
                var user = await _context.Users.Include(u => u.Photos).SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                

                if(user != null)
                {
                    var activitiesHosted = await _context.Activities.Where(a => a.Attendees.Any(aa => aa.AppUserId == user.Id && aa.IsHost))
                    .ToListAsync();
                 
                    return Result<List<Activity>>.Success(activitiesHosted);
                }
                Console.WriteLine("Not catching any user!!!!!!!!!!!!!!" + user);

                return Result<List<Activity>>.Failure("Failed to Find User");
            }
        }
    }
}