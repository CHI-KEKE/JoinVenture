using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> UserManager)
        {
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
            if(context.Activities.Any()) return;

            var activities = new List<Activity>
            {
                new Activity
                {
                    Title = "Xpark 都會型水生公園遊",
                    Date = DateTime.UtcNow.AddMonths(-2),
                    Description = "福爾摩沙展區大水槽，呈現出靜謐的浪漫氛圍，讓人沉浸感受不同的光線變化",
                    Category = "",
                    City = "桃園",
                    Venue = "Xpark",
                    Image = "https://d1pjwdyi3jyxcs.cloudfront.net/cool-blue-wolf.",
                    Followers = 421,
                    Tickets = 100,
                    Attendees = new List<ActivityAttendee>
                    {
                        new ActivityAttendee
                        {
                            AppUser = users[0],
                            IsHost = true
                        },
                        new ActivityAttendee
                        {
                            AppUser = users[1],
                            IsHost = false
                        },
                    }
                },
                new Activity
                {
                    Title = "Past Activity 2",
                    Date = DateTime.UtcNow.AddMonths(-1),
                    Description = "Activity 1 month ago",
                    Category = "culture",
                    City = "Paris",
                    Venue = "Louvre",
                    Image = "https://d1pjwdyi3jyxcs.cloudfront.net/acc1.jpg",
                    Followers = 300,
                    Tickets = 135,
                    Attendees = new List<ActivityAttendee>
                    {
                        new ActivityAttendee
                        {
                            AppUser = users[0],
                            IsHost = false
                        },
                        new ActivityAttendee
                        {
                            AppUser = users[1],
                            IsHost = true
                        },
                    }
                },
                new Activity
                {
                    Title = "Future Activity 1",
                    Date = DateTime.UtcNow.AddMonths(1),
                    Description = "Activity 1 month in future",
                    Category = "culture",
                    City = "London",
                    Venue = "Natural History Museum",
                    Image = "https://d1pjwdyi3jyxcs.cloudfront.net/ivbjio8-cool-wolf-images.",
                    Followers = 100,
                    Tickets = 25,
                    Attendees = new List<ActivityAttendee>
                    {
                        new ActivityAttendee
                        {
                            AppUser = users[1],
                            IsHost = false
                        },
                        new ActivityAttendee
                        {
                            AppUser = users[2],
                            IsHost = true
                        },
                    }
                },
                new Activity
                {
                    Title = "Future Activity 2",
                    Date = DateTime.UtcNow.AddMonths(2),
                    Description = "Activity 2 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "O2 Arena",
                    Image = "https://d1pjwdyi3jyxcs.cloudfront.net/logo.jpg",
                    Followers = 400,
                    Tickets = 200
                },
                new Activity
                {
                    Title = "Future Activity 3",
                    Date = DateTime.UtcNow.AddMonths(3),
                    Description = "Activity 3 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Another pub",
                    Image = "https://d1pjwdyi3jyxcs.cloudfront.net/acc2.jpg",
                    Followers = 100,
                    Tickets = 135
                },
                new Activity
                {
                    Title = "Future Activity 4",
                    Date = DateTime.UtcNow.AddMonths(4),
                    Description = "Activity 4 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Yet another pub",
                    Image = "https://d1pjwdyi3jyxcs.cloudfront.net/acc3.jpg",
                    Followers = 100,
                    Tickets = 100
                },
                new Activity
                {
                    Title = "Future Activity 5",
                    Date = DateTime.UtcNow.AddMonths(5),
                    Description = "Activity 5 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Just another pub",
                    Image = "https://d1pjwdyi3jyxcs.cloudfront.net/women1.jpg",
                    Followers = 100,
                    Tickets = 100
                },
                new Activity
                {
                    Title = "Future Activity 6",
                    Date = DateTime.UtcNow.AddMonths(6),
                    Description = "Activity 6 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "Roundhouse Camden",
                    Image = "https://d1pjwdyi3jyxcs.cloudfront.net/women2.jpg",
                    Followers = 200,
                    Tickets = 70
                },
                new Activity
                {
                    Title = "Future Activity 7",
                    Date = DateTime.UtcNow.AddMonths(7),
                    Description = "Activity 2 months ago",
                    Category = "travel",
                    City = "London",
                    Venue = "Somewhere on the Thames",
                    Image = "https://d1pjwdyi3jyxcs.cloudfront.net/women3.jpg",
                    Followers = 200,
                    Tickets = 135
                },
                new Activity
                {
                    Title = "Future Activity 8",
                    Date = DateTime.UtcNow.AddMonths(8),
                    Description = "Activity 8 months in future",
                    Category = "film",
                    City = "London",
                    Venue = "Cinema",
                    Image = "https://d1pjwdyi3jyxcs.cloudfront.net/women4.jpg",
                    Followers = 50,
                    Tickets = 50
                }
            };

            await context.Activities.AddRangeAsync(activities);
            await context.SaveChangesAsync();
        }
    }
}