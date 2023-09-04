using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Helpers;
using API.Provider;
using API.Service;
using Application.Activities;
using Application.Events;
using Application.Interface;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class ActivitiesController:BaseController
    {
        private readonly IMapper _mapper;
        private readonly SaveUploadedFileService _fileService;
        private readonly JsonProvider _jsonProvider;
        private readonly IResponseCacheService _responseCacheService;

        public ActivitiesController(IMapper mapper, SaveUploadedFileService fileService,JsonProvider jsonProvider,IResponseCacheService responseCacheService)
        {
            _responseCacheService = responseCacheService;
            _jsonProvider = jsonProvider;
            _fileService = fileService;
            _mapper = mapper;
            
        }

        [ActivityListCacheAttribute(600)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> GetActivities()
        {
            return Ok(await Mediator.Send(new List.Query()));
        }

        [ActivityDetailCache(600)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivity(int id)
        {
            return Ok(await Mediator.Send(new Details.Query{Id = id}));
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromForm] ActivityCreateRequestDto activityCreateRequestDto)
        {
            var jsonData = JsonSerializer.Serialize(activityCreateRequestDto);
            Console.WriteLine(jsonData+"nice catch@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

            var activityTransformed = _mapper.Map<ActivityCreateRequestDto, Activity>(activityCreateRequestDto);
            Console.WriteLine(activityTransformed+"Transformed done##################################################");
            Console.WriteLine(activityCreateRequestDto.Image+"Image!?????????????????????????????????????????????");

            //Image Handling
            string CloudFrontImagePath = await _fileService.SaveUploadedFileMethod(activityCreateRequestDto.Image);
            activityTransformed.Image = CloudFrontImagePath;
            Console.WriteLine(CloudFrontImagePath+"Image done##################################################");


            var ticketPackagesJson = HttpContext.Request.Form["ticketPackages"];
            var ticketPackages = _jsonProvider.Deserialize<List<TicketPackageDTO>>(ticketPackagesJson);

            var ticketPackagesTransformed = _mapper.Map<List<TicketPackageDTO>, List<TicketPackage>>(ticketPackages);

            // TicketPackages Handling
            if (ticketPackagesTransformed != null)
            {
                foreach (var ticketPackagetrans in ticketPackagesTransformed)
                {
                    var ticketPackage = new TicketPackage
                    {
                        Title = ticketPackagetrans.Title,
                        Price = (int)ticketPackagetrans.Price,
                        Description = ticketPackagetrans.Description,
                        Count = ticketPackagetrans.Count,
                        ValidatedDateStart = ticketPackagetrans.ValidatedDateStart,
                        ValidatedDateEnd = ticketPackagetrans.ValidatedDateEnd,
                        BookingAvailableStart = ticketPackagetrans.BookingAvailableStart,
                        BookingAvailableEnd = ticketPackagetrans.BookingAvailableEnd,
                    };

                    for (int i = 0; i < ticketPackage.Count; i++) 
                    {
                        var ticket = new Ticket
                        {
                            Status = "Available", 
                            UserId = null,
                            Version =null,
                        };

                        ticketPackage.Tickets.Add(ticket);
                    }

                    // Add the TicketPackage to the Activity
                    activityTransformed.TicketPackages.Add(ticketPackage);
                }
            Console.WriteLine(ticketPackagesTransformed+"TicketPackages done##################################################");

            }

            try
            {
                var NewActivity = await Mediator.Send(new Create.Command {Activity = activityTransformed});
                await _responseCacheService.RemoveDataAsync("/Activities");

                 return Ok(NewActivity);

            }
            catch(Exception ex)
            {
                Console.Error.WriteLine($"Error creating activity: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating activity"); 

            }

        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]

        public async Task<IActionResult> EditActivity(int id, Activity activity)
        {
            activity.Id = id;
            return Ok(await Mediator.Send(new Edit.Command {Activity = activity}));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            return Ok(await Mediator.Send(new Delete.Command {Id = id}));
        }
        
        [HttpPost("{id}/follow")]
        public async Task<IActionResult> Follow(int id)
        {
            var result = await Mediator.Send(new UpdateFollowers.Command{Id =id});

            Console.WriteLine(result);
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Error);
            }
        }
    }
}