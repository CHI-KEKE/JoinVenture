using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interface;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UpdateAttendance
    {
        public class Command :IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly DataContext _context;
            public Handler(DataContext context,IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities
                    .Include(a => a.Attendees)
                    .ThenInclude(u => u.AppUser)
                    .SingleOrDefaultAsync(x => x.Id == request.Id);
                Console.WriteLine("after getting activities~~~~~~~~~~~" + activity);
                if(activity == null) return null;



                Console.WriteLine("before GetUSerName~~~~~~~~~~~~~~" + activity);
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                Console.WriteLine(user);


                
                var hostUserName = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;
                Console.WriteLine(hostUserName);
                var attendance = activity.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);
                Console.WriteLine(attendance);
                if(attendance != null && hostUserName == user.UserName)
                    activity.IsCancelled = !activity.IsCancelled;
                Console.WriteLine("not attendance and not host");

                if(attendance != null && hostUserName != user.UserName)
                    activity.Attendees.Remove(attendance);
                Console.WriteLine("not attendance and not host");
                if(attendance == null)
                {
                    Console.WriteLine("try to attend");
                    attendance = new ActivityAttendee
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };

                    activity.Attendees.Add(attendance);
                }

                var result = await _context.SaveChangesAsync() > 0;

                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating");
            }
        }
    }
}