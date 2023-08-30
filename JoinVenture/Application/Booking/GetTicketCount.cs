using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Tickets;
using Application.Events;
using AutoMapper;
using MediatR;
using Persistence;




namespace Application.Booking
{
    public class GetTicketCount
    {
         public class Query : IRequest<List<TicketRemainCountDto>>
        {
            public string ActivityId {get;set;}
            public List<TicketCheckingDto> TicketCheckingDtos { get; set; }
        }


        public class Handler : IRequestHandler<Query, List<TicketRemainCountDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;

            public Handler(DataContext context,IMapper mapper,IMediator mediator)
            {
                _mediator = mediator;
                _mapper = mapper;
                _context = context;
            }
            public async Task<List<TicketRemainCountDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                Console.WriteLine("I am inside getTicket Count Method...............................................................");
                List<TicketRemainCountDto> TicketCounts = new List<TicketRemainCountDto>(); 


                if (int.TryParse(request.ActivityId, out int activityId))
                {
                    var Activity = await _mediator.Send(new Details.Query { Id = activityId });


                    foreach(var packageInfoDto in request.TicketCheckingDtos)
                    {
                        Console.WriteLine("Inside get ticket Count loop...............................................................");

                        var TicketPackage = Activity.TicketPackages.SingleOrDefault(tp => tp.Title == packageInfoDto.PackageTitle);


                        var RemainTicketCountsForPackage = TicketPackage.Tickets
                        .Where(t => t.Status == "Available")
                        .Count();

                        var TicketRemainCountDto = new TicketRemainCountDto
                        {
                            PackageTitle =packageInfoDto.PackageTitle,
                            Count = RemainTicketCountsForPackage
                        };


                        TicketCounts.Add(TicketRemainCountDto);
                    }

                    return TicketCounts;

                }

                return TicketCounts;


            }


        }       
    }
}