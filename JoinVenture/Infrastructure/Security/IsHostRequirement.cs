using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class IsHostRequirement:IAuthorizationRequirement
    {
        
    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IsHostRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userID = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine(userID+"Checking Deleting UserId********************************************************************");

            if(userID == null) return Task.CompletedTask;

            var activityIdString = _httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value as string;
            if (int.TryParse(activityIdString, out int activityId))
            {
                Console.WriteLine(activityId + " Checking Deleting ActivityId)))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))");
            }

            // var attendee = _dbContext.ActivityAttendees.FindAsync(userID,activityId).Result;
            var attendee = _dbContext.ActivityAttendees
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.AppUserId == userID && x.ActivityId == activityId)
                .Result;




            Console.WriteLine(attendee+"Checking Deleting Attendee********************************************************************");

            if(attendee == null) return Task.CompletedTask;

            if(attendee.IsHost) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    
}