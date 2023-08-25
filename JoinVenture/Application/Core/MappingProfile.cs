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

            CreateMap<OrderDto,Order>()
            .ForMember(dest => dest.BookedTicketPackages, opt => opt.MapFrom(src => src.BookedTicketPackages))
            .ForMember(dest => dest.Id, opt => opt.Ignore());



        }
    }
}