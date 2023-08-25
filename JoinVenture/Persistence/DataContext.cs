using System.Security.Cryptography;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext:IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Activity> Activities{get;set;}

        public DbSet<ActivityAttendee> ActivityAttendees{get;set;}
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments{get;set;}
        public DbSet<TicketPackage> TicketPackages{get;set;}

        public DbSet<Order> Orders {get;set;}
        public DbSet<BookedTicketPackage> BookedTicketPackages {get;set;}

        public DbSet<Ticket> Tickets {get;set;}


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new {aa.AppUserId,aa.ActivityId}));

            builder.Entity<ActivityAttendee>()
            .HasOne(u => u.AppUser)
            .WithMany(a => a.Activities)
            .HasForeignKey(aa => aa.AppUserId);

            builder.Entity<ActivityAttendee>()
            .HasOne(a => a.Activity)
            .WithMany(u => u.Attendees)
            .HasForeignKey(aa => aa.ActivityId);

            builder.Entity<Comment>()
                .HasOne(a => a.Activity)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<TicketPackage>()
                .HasOne(a => a.Activity)
                .WithMany(t => t.TicketPackages)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BookedTicketPackage>()
                .HasOne(o => o.Order)
                .WithMany(b => b.BookedTicketPackages)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}