using MediatR;
using Microsoft.AspNetCore.SignalR;


namespace API.SignalR
{
    public class TicketHub:Hub
    {
        private readonly IMediator _mediator;
        public TicketHub(IMediator mediator)
        {
            _mediator = mediator;
        }       
        public async Task UpdateTickets(Application.Booking.GetTicketCount.Query query)
        {
            var TicketCountData = await _mediator.Send(query);

            await Clients.Group(query.ActivityId.ToString())
            .SendAsync("ReturnUpdatedTickets", TicketCountData);
        }       

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];

            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

        } 
    }
}