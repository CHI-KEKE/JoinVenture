using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Application.Events;
using Application.Booking;
using Persistence;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly DataContext _context;
        public ChatHub(IMediator mediator,DataContext context)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task SendComment(Application.Comments.Create.Command command)
        {
            var comment = await _mediator.Send(command);

            int activityId = int.Parse(command.ActivityId);

            var activityNow = await _mediator.Send(new Details.Query{Id = activityId});
            await Clients.Group(command.ActivityId.ToString())
            .SendAsync("Receive Comments", comment.Value);

            await Clients.Group($"{command.ActivityId.ToString()}_follow")
            .SendAsync("Follower Only Messages", comment.Value,activityNow);

            
        }
        public async Task Booking(Booking.Command command)
        {
             var ticketCount = await _mediator.Send(command);
             await Clients.Group(command.ActivityId.ToString())
            .SendAsync("UpdateTicketCount", ticketCount);           
        }

        public async Task FollowActivity(string accesstoken)
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            await Groups.AddToGroupAsync(accesstoken, $"{activityId}_follow");
        }


        public async Task LoadHostedNotifications(ListHosted.Query query)
        {
            Console.WriteLine("What the query would be like...?" + query);
                var activities = await _mediator.Send(query);
                Console.WriteLine(activities);
                
                await Clients.Caller.SendAsync("ReceiveHostedActivities", activities.Value);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            var user = Context.User;

            
            if (user.Identity.IsAuthenticated)
            {
                string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string username = user.Identity.Name;
                Console.WriteLine(username + "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=");

                var TheConnectingUser = _context.Users.Where(u => u.Id == userId).Include(u => u.Activities).SingleOrDefault();

                foreach(var activity in TheConnectingUser.Activities)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"{activity.ActivityId}_follow");
                }
            }

            if(string.IsNullOrEmpty(activityId))
            {
                Console.WriteLine( "no Activity specified");
            }

            else{
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

            var ActivityInfo = await _mediator.Send(new Details.Query { Id = int.Parse(activityId) });

            var result = await _mediator.Send(new Application.Comments.List.Query{ActivityId = int.Parse(activityId)});


            await Clients.Caller.SendAsync("LoadTicketCount", ActivityInfo.Tickets);
            await Clients.Caller.SendAsync("LoadComments", result.Value);
            }

        }


    }
}