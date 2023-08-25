using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Application.Events;
using Application.Booking;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(Application.Comments.Create.Command command)
        {
            var comment = await _mediator.Send(command);

            await Clients.Group(command.ActivityId.ToString())
            .SendAsync("Receive Comments", comment.Value);

            await Clients.Group($"{command.ActivityId.ToString()}_follow")
            .SendAsync("Follower Only Messages", comment.Value);
        }

        public async Task Booking(Booking.Command command)
        {
             var ticketCount = await _mediator.Send(command);
            Console.WriteLine(ticketCount + "What are Booking return !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
             await Clients.Group(command.ActivityId.ToString())
            .SendAsync("UpdateTicketCount", ticketCount);           
        }

        public async Task FollowActivity()
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{activityId}_follow");
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


            if(string.IsNullOrEmpty(activityId))
            {
                Console.WriteLine( "Not a DetailPage now");
            }
            else{
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

            var ActivityInfo = await _mediator.Send(new Details.Query { Id = Guid.Parse(activityId) });


            var result = await _mediator.Send(new Application.Comments.List.Query{ActivityId = Guid.Parse(activityId)});

            Console.WriteLine( ActivityInfo.Tickets + "What are Loading return !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            await Clients.Caller.SendAsync("LoadTicketCount", ActivityInfo.Tickets);
            await Clients.Caller.SendAsync("LoadComments", result.Value);
            }

        }


        // public async Task LoadHostedActivities()
        // {

        // }
    }
}