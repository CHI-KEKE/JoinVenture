using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using Application.Comments;
using Application.Orders;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity,Activity>();

            CreateMap<ActivityAttendee,Profiles.Profile>()
            .ForMember(d => d.ShowName, o => o.MapFrom(s => s.AppUser.ShowName))
            .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio));
            
            CreateMap<Activity,ActivityDto>()
             .ForMember(d => d.HostUserName, o => o.MapFrom(s => s.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName));

            

            


            CreateMap<Comment,CommentDto>()
            .ForMember(d => d.ShowName, o => o.MapFrom(s => s.Author.ShowName))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.Author.UserName))
            .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url));


            CreateMap<BookedTicketPackageDto,BookedTicketPackage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Order, opt => opt.Ignore()); 


            CreateMap<string, Ticket>() // Map a single string ticketId to a Ticket
            .ConvertUsing<TicketIdToTicketConverter>();


            CreateMap<OrderDto,Order>()
            .ForMember(dest => dest.BookedTicketPackages, opt => opt.MapFrom(src => src.BookedTicketPackages))
            .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.Tickets))
            .ForMember(dest => dest.Id, opt => opt.Ignore());






            CreateMap<ActivityCreateRequestDto, Activity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore mapping the Id
                .ForMember(dest => dest.Followers, opt => opt.Ignore()) // Ignore mapping the Followers property
                .ForMember(dest => dest.Tickets, opt => opt.Ignore()) // Ignore mapping the Tickets property
                .ForMember(dest => dest.IsCancelled, opt => opt.Ignore()) // Ignore mapping the IsCancelled property
                .ForMember(dest => dest.Attendees, opt => opt.Ignore()) // Ignore mapping the Attendees property
                .ForMember(dest => dest.Comments, opt => opt.Ignore()) // Ignore mapping the Comments property
                .ForMember(dest => dest.TicketPackages, opt => opt.Ignore()) // Ignore mapping the TicketPackages property
                .ForMember(dest => dest.Image, opt => opt.Ignore()); // Map Image property from IFormFile to string

            CreateMap<TicketPackageDTO, TicketPackage>()
                    .ForMember(dest => dest.ValidatedDateStart, opt => opt.MapFrom(src => DateTime.Parse(src.ValidatedDateStart)))
                    .ForMember(dest => dest.ValidatedDateEnd, opt => opt.MapFrom(src => DateTime.Parse(src.ValidatedDateEnd)))
                    .ForMember(dest => dest.BookingAvailableStart, opt => opt.MapFrom(src => DateTime.Parse(src.BookingAvailableStart)))
                    .ForMember(dest => dest.BookingAvailableEnd, opt => opt.MapFrom(src => DateTime.Parse(src.BookingAvailableEnd)));
   
        }
    }
}