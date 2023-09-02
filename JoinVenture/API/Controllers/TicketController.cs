using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Tickets;
using Application.Events;
using Application.Booking;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using Persistence;
using System.Text.Json;
using API.Service;
using Application.Interface;

namespace API.Controllers
{
    public class TicketController:BaseController
    {

        private readonly DataContext _dataContext;
        private readonly TicketBookingService _ticketBookingService;

        public TicketController(DataContext dataContext,TicketBookingService ticketBookingService)
        {
            _ticketBookingService = ticketBookingService;
            _dataContext = dataContext;
            
        }


        [HttpPost("{id}/book")]
        public async Task<IActionResult> GetAvailibleTickets (int id, List<DTOs.Tickets.TicketCheckingDto> ticketCheckingDtos)
        {

            var ActivityId = id;
            var Activity = await Mediator.Send(new Details.Query{Id = ActivityId});


            List<AvailibleTicketsDto> ListOfSelectedTicketsToReturn = new List<AvailibleTicketsDto>(); 
            
            foreach(var packageInfoDto in ticketCheckingDtos)
            {
                var TicketPackage = Activity.TicketPackages.SingleOrDefault(tp => tp.Title == packageInfoDto.PackageTitle);

                var (Bookedtickets, message) = await _ticketBookingService.BookTickets(TicketPackage,packageInfoDto.Quantity);
                Console.WriteLine($"outside transaction {Bookedtickets} & {message}!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
  
                if(Bookedtickets != null)

                {
                    foreach(var ticket in Bookedtickets)
                    {
                        var RandomPickAvailibleTicketForReturn = new AvailibleTicketsDto
                        {
                                PackageTitle = packageInfoDto.PackageTitle,
                                TicketId = ticket.Id,
                                Stauts = ticket.Status,
                                ExpiredAt = ticket.ExpiredAt,
                                UserId = ticket.UserId,                    
                        };

                        ListOfSelectedTicketsToReturn.Add(RandomPickAvailibleTicketForReturn);

                    }
                }

                if(message == "409")
                {
                    return Conflict(message);
                }
                if(message == "400")
                {
                    return BadRequest(message);
                }

                if(message == "504")
                {
                    return Problem(message);
                }


                
                
            }
            
            Console.WriteLine($"before return the tickcetListDto {ListOfSelectedTicketsToReturn}!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            return Ok(ListOfSelectedTicketsToReturn);
            
        }       


        public static List<int> GenerateRandomIndices(int maxIndex, int count)
        {
            if (count >= maxIndex)
            {
                throw new ArgumentException("Count must be less than the maximum index.");
            }

            Random random = new Random();
            List<int> indices = new List<int>();

            while (indices.Count < count)
            {
                int newIndex = random.Next(0, maxIndex);
                if (!indices.Contains(newIndex))
                {
                    indices.Add(newIndex);
                }
            }

            return indices;
        }

        [HttpPost]
        public async Task<IActionResult> TicketToSuccess (List<AvailibleTicketsDto> ticketsDto)
        {
            foreach(var dto in ticketsDto)
            {
                var ticket = _dataContext.Tickets.SingleOrDefault(t => t.Id == dto.TicketId);
                if(ticket.Status == "Pending")
                {
                    ticket.Status = "Success";
                    _dataContext.SaveChanges();
                    var ticketJson = JsonSerializer.Serialize(ticket);
                    Console.WriteLine(ticketJson);
                }
                else{
                    return BadRequest("Ticket has Release due to Expiration");

                }


            }


            return Ok("All tickets stats changed to Success successfully!"); 
        }


    }
}