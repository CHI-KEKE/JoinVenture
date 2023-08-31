using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;

namespace API.Service
{
    public class ExpiredTicketCleanUpService : IHostedService, IDisposable
    {


        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public ExpiredTicketCleanUpService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10)); // Interval
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {

            Console.WriteLine( DateTime.Now.ToString() + "Start Cleanning!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                var currentTime = DateTime.Now;

                // Query for tickets with expired reservations
                var expiredTickets = dbContext.Tickets
                    .Where(t => t.Status == "Pending" && t.ExpiredAt <= currentTime)
                    .ToList();


                if(expiredTickets != null)
                {
                    // Release expired reservations
                    foreach (var ticket in expiredTickets)
                    {
                        ticket.Status = "Available";
                        ticket.ExpiredAt = null; // Clear the expiration time
                        ticket.UserId = null;

                        Console.WriteLine(ticket.Status + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        Console.WriteLine(ticket.ExpiredAt + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        Console.WriteLine(ticket.UserId + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        Console.WriteLine(ticket.Id + "Was Released!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    }

                    dbContext.SaveChanges();

                    Console.WriteLine( DateTime.Now.ToString() + " Clean Done!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                }
                else{
                    Console.WriteLine("No Expired Ticket Need to Clean!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }


            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}