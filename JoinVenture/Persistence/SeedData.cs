using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class SeedData
    {
        public static async Task SeedNewData(DataContext context, UserManager<AppUser> UserManager)
        {

            //user
            var users = new List<AppUser>

            {
                new AppUser{ShowName = "Allen",UserName = "allen",Email = "allen@gmail.com"},
                new AppUser{ShowName = "Gina",UserName = "gina",Email = "gina@gmail.com"},
                new AppUser{ShowName = "Moji",UserName = "moji",Email = "moji@gmail.com"},

            };

            if(!UserManager.Users.Any())
            {
                foreach(var user in users)
                {
                    await UserManager.CreateAsync(user,"Pa$$w0rd");
                }
            }

            if(!context.Photos.Any())
            {
                var userAllen = context.Users.FirstOrDefault(u => u.ShowName == "Allen");
                var photo = new Photo
                {
                    Url = "https://d1pjwdyi3jyxcs.cloudfront.net/JoinVenture/cat.jpg",
                    IsMain = true // Set this as the main photo if needed
                };

                // Add the Photo to the user's Photos collection
                userAllen.Photos.Add(photo);

                var userMoji = context.Users.FirstOrDefault(u => u.ShowName == "Moji");
                var photoMoji = new Photo
                {
                    Url = "https://d1pjwdyi3jyxcs.cloudfront.net/JoinVenture/moji.jpg",
                    IsMain = true // Set this as the main photo if needed
                };

                // Add the Photo to the user's Photos collection
                userMoji.Photos.Add(photoMoji);

                var userGina = context.Users.FirstOrDefault(u => u.ShowName == "Gina");
                var photoGina = new Photo
                {
                    Url = "https://d1pjwdyi3jyxcs.cloudfront.net/JoinVenture/ginacat.png",
                    IsMain = true // Set this as the main photo if needed
                };

                // Add the Photo to the user's Photos collection
                userGina.Photos.Add(photoGina);

                context.SaveChanges();
            }



            
            //Activities

            if (!context.Activities.Any())
            {
                var activitiesJson = File.ReadAllText("../Persistence/SeedData/activities.json");
                var activities = JsonSerializer.Deserialize<List<Activity>>(activitiesJson);


                DateTime futureDate = DateTime.UtcNow.AddDays(7);

                foreach(var activity in activities)
                {

                    //Seed Date of Activity
                    activity.Date = futureDate;
                    futureDate = futureDate.AddDays(1);

                    //Seed TicketPackage each with three
                    var ticketPackages = new List<TicketPackage>
                    {
                        new TicketPackage{
                            Title = "早鳥票",
                            Price = 100,  
                            Description = "後排座位",
                            Count = 50,
                            Activity= activity,
                            ValidatedDateStart = activity.Date,
                            ValidatedDateEnd = activity.Date.AddHours(2),
                            BookingAvailableStart = activity.Date.AddMonths(-1),
                            BookingAvailableEnd = activity.Date.AddDays(-1),
                        },
                        new TicketPackage{
                            Title = "普通票",
                            Price = 200,  
                            Description = "後排座位",
                            Count = 50,
                            Activity= activity,
                            ValidatedDateStart = activity.Date,
                            ValidatedDateEnd = activity.Date.AddHours(2),
                            BookingAvailableStart = activity.Date.AddMonths(-1),
                            BookingAvailableEnd = activity.Date.AddDays(-1),
                        },
                        new TicketPackage{
                            Title = "豪華票",
                            Price = 300,  
                            Description = "前排座位",
                            Count = 50,
                            Activity= activity,
                            ValidatedDateStart = activity.Date,
                            ValidatedDateEnd = activity.Date.AddHours(2),
                            BookingAvailableStart = activity.Date.AddMonths(-1),
                            BookingAvailableEnd = activity.Date.AddDays(-1),
                        },

                    };

                    foreach (var package in ticketPackages)
                    {
                        activity.TicketPackages.Add(package);
                    }



                    //Seed activity host
                    Random random = new Random();
                    int randomNumber = random.Next(2);
                    var attendance = new ActivityAttendee
                    {
                        AppUser = users[randomNumber],
                        Activity = activity,
                        IsHost = true
                    };

                    activity.Attendees.Add(attendance);
                    
                }

                await context.Activities.AddRangeAsync(activities);
                await context.SaveChangesAsync();               
            }


            if (!context.Tickets.Any())
            {
                var random = new Random();

                foreach (var activity in context.Activities.Include(a => a.TicketPackages).ThenInclude(tp =>tp.Tickets))
                {

                    foreach (var ticketPackage in activity.TicketPackages)
                    {
                        for (int i = 0; i < 20; i++) 
                        {
                            var ticket = new Ticket
                            {
                                Status = "Available", // You can set the initial status as needed
                                ExpiredAt = null,
                                UserId = null,
                                TicketPackage = ticketPackage
                            };

                            context.Tickets.Add(ticket);
                        }
                    }
                }

                context.SaveChanges();
            }


        }
    }
}