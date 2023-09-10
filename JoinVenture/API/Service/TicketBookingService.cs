using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Service
{
    public class TicketBookingService
    {

        private readonly IMediator _mediator;
        private readonly IUserAccessor _userAccessor;

        private readonly DataContext _dbContext;
        public TicketBookingService(DataContext dbContext,IMediator mediator,IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _mediator = mediator;
            _dbContext = dbContext;
        }


        public async Task<(List<Ticket>, string,string)> BookTickets(TicketPackage ticketPackage,int quantity)
        {

            // var transactionTimeout = TimeSpan.FromSeconds(10);
            var transactionCancellationTokenSource = new CancellationTokenSource(3000);

            
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(transactionCancellationTokenSource.Token))
            {

                    // int maxRetries = 3; 
                    // int retryDelayMilliseconds = 100; 
                    string errorCondition = null;
                
                // for (int retryCount = 0; retryCount < maxRetries; retryCount++)
                // {
                    // Console.WriteLine($"retry count {retryCount}!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        try
                        {

                            _dbContext.Database.ExecuteSqlRaw("SET LOCK_TIMEOUT 10000");

                            // var AvailibleTicketList = ticketPackage.Tickets.Where(t => t.Status == "Available").Take(quantity).ToList();

                            // var targetTicket = ticketPackage.Tickets.FirstOrDefault(t => t.Id == Guid.Parse("9EBCF90A-6173-432F-97C4-08DBA6EE514E"));
                            // var targetTicket = _dbContext.Tickets
                            //     .FromSqlInterpolated($@"
                            //         SELECT *
                            //         FROM Tickets WITH (UPDLOCK)
                            //         WHERE Id = {Guid.Parse("FF057E1C-5FCF-4B89-97C9-08DBA6EE514E")}
                            //     ")
                            //     .FirstOrDefault();


                            // var availableTicketList = ticketPackage.Tickets
                            //     .Where(t => t.Status == "Available")
                            //     .Take(quantity)
                            //     .ToList();

                            // var ticketIdsToLock = availableTicketList.Select(t => t.Id).ToList();
                            // var ticketIdStrings = ticketIdsToLock.Select(id => $"'{id}'");
                            // var ticketIdList = string.Join(",", ticketIdStrings);
                            // var rawSql = $@"
                            //                 SELECT *
                            //                 FROM Tickets WITH (UPDLOCK)
                            //                 WHERE Id IN ({ticketIdList})
                            //             ";

                            // var lockedTickets = _dbContext.Tickets.FromSqlRaw(rawSql).ToList();

                            var random = new Random();
                            var availableTicketIds = ticketPackage.Tickets
                                .Where(t => t.Status == "Available")
                                .OrderBy(t => random.Next()) // Randomly order the available tickets
                                .Take(quantity)
                                .Select(t => t.Id)
                                .ToList();
                            
                            if(availableTicketIds.Count != quantity)
                            {
                                return (null,"400","Not Enough Tickets Availible");
                            }


                            //testing other lock not UPDLOCK
                            var lockedTickets = _dbContext.Tickets
                                .FromSqlRaw($@"
                                    SELECT *
                                    FROM Tickets WITH (UPDLOCK)   
                                    WHERE Id IN ({string.Join(",", availableTicketIds.Select(id => $"'{id}'"))})
                                ")
                                .ToList();




                            // await Task.Delay(8000);
                            // Thread.Sleep(11000);



                            if (lockedTickets.Count == quantity)               //檢查是否真的抓到指定得票數
                            {
                                Console.WriteLine("the count is match!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                                foreach(var ticket in lockedTickets)
                                {
                                        var dbTicket = _dbContext.Tickets.FirstOrDefault(t => t.Id == ticket.Id && t.Status == "Available");   //這張票是否還是availible

                                        if (dbTicket != null)
                                        {
                                            Console.WriteLine("ticket still availible!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                                            ticket.Status = "Pending";
                                            ticket.ExpiredAt = DateTime.Now.AddSeconds(10);
                                            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                                            Console.WriteLine(user + "Inside the transaction Update process and grab user successfully!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                                            ticket.UserId = user.Id;
                                            // var UpdatedTicket = await _mediator.Send(new Application.Booking.Edit.Command {Ticket = ticket});

                                            _dbContext.SaveChanges();
                                        }

                                    

                                        else
                                        {

                                            errorCondition = "Some of the selected tickets are no longer available.";
                                            throw new DbUpdateConcurrencyException("Some of the selected tickets are no longer available.");

                                        }

                                }


                                transaction.Commit();                                   //成功狀況 return Tickets with this package!
                                errorCondition = null;
                                Console.WriteLine("just committed!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                                return (lockedTickets,null,null);
                            }
                            else
                            {
                                // Handle the case where there are not enough available tickets
                                return (null,"400","Not Enough Tickets Availible");
                                // Thread.Sleep(retryDelayMilliseconds);
                            }

                        }
                        catch (OperationCanceledException)
                        {
                            // Transaction timed out
                            transaction.Rollback();
                            return (null, "Timeout","Timeout");
                        }
                        catch (DbUpdateConcurrencyException ex)                                    //歸類為Concurrency Conflict, 就REtry
                        {
                            // Handle concurrency conflict (another user updated the same ticket)
                            // Log the conflict or take appropriate action
                            transaction.Rollback();
                            // Wait for a while before retrying
                            // Thread.Sleep(retryDelayMilliseconds);
                            if(errorCondition != null)
                            {
                                return (null,"409",errorCondition);
                            }
                            return (null,"409",ex.Message);
                        }
                        catch (Exception ex)                                                        ////其他問題 先不retry
                        {
                            return (null,"500",ex.Message);
                            // break; 
                        }

        
            }

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