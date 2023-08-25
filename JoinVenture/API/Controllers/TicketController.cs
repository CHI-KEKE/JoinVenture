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

namespace API.Controllers
{
    public class TicketController:BaseController
    {
        [AllowAnonymous]
        [HttpPost("{id}/book")]
        public async Task<IActionResult> GetAvailibleTickets (Guid id, List<TicketCheckingDto> ticketCheckingDtos)
        {

            //Get Specific Activity

            var ActivityId = id;
            var Activity = await Mediator.Send(new Details.Query{Id = ActivityId});


            List<AvailibleTicketsDto> ListOfSelectedTicketsToReturn = new List<AvailibleTicketsDto>(); 
            
            // RequiredAttribute Packages from this Activity

            foreach(var packageInfoDto in ticketCheckingDtos)
            {
                //Get Availible Tickets
                var TicketPackage = Activity.TicketPackages.SingleOrDefault(tp => tp.Title == packageInfoDto.PackageTitle);
                var AvailibleTicketList = TicketPackage.Tickets.Where(t => t.Status == "Available").ToList();

                //Random Indices
                List<int> randomIndices = GenerateRandomIndices(AvailibleTicketList.Count, packageInfoDto.Quantity);

                foreach (int index in randomIndices)
                {
                    //Random Avalible Tickets
                    var RandomPickAvailibleTicket = AvailibleTicketList[index];
                    Console.WriteLine("!!!!!!!!!!!!!!!!!Before Changed Ticket Id : " + RandomPickAvailibleTicket.Id);

                    //Update The Picked Tickets
                    DateTime now = DateTime.Now;
                    DateTime tenMinutesFromNow = now.AddMinutes(10);

                    RandomPickAvailibleTicket.Status = "Pending";
                    RandomPickAvailibleTicket.ExpiredAt = tenMinutesFromNow;

                    var RandomPickAvailibleTicketIsPending = await Mediator.Send(new Application.Booking.Edit.Command {Ticket = RandomPickAvailibleTicket});
                    
                    Console.WriteLine("!!!!!!!!!!!!!!!!!After Changed Ticket Id : " + RandomPickAvailibleTicketIsPending.Id);


                    Console.WriteLine(RandomPickAvailibleTicketIsPending.Status + "Status has Changed!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    Console.WriteLine(RandomPickAvailibleTicketIsPending.UserId + "User has Filled!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    Console.WriteLine(RandomPickAvailibleTicketIsPending.ExpiredAt + "There is Expriation Time!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    
                    //Modeling for Return to FrontEnd
                    var RandomPickAvailibleTicketForReturn = new AvailibleTicketsDto
                    {
                        PackageTitle = packageInfoDto.PackageTitle,
                        TicketId = RandomPickAvailibleTicketIsPending.Id,
                        Stauts = RandomPickAvailibleTicketIsPending.Status,
                        ExpiredAt = RandomPickAvailibleTicketIsPending.ExpiredAt,
                        UserId = RandomPickAvailibleTicketIsPending.UserId,
                    };

                    ListOfSelectedTicketsToReturn.Add(RandomPickAvailibleTicketForReturn);

                }

                
                
            }



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
    }
}