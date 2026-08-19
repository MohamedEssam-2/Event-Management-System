using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.WishlistDTO;
using Data_Access_Layer.Models;

namespace Business_Logic_Layer.Mapping
{
    public class WishlistProfile:Profile
    {
        public WishlistProfile()
        {
            CreateMap<Wishlist, ReadWishlistDTO>()
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(dest => dest.EventDescription, opt => opt.MapFrom(src => src.Event.Description))
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.Event.Date))
                .ForMember(dest => dest.EventLocation, opt => opt.MapFrom(src => src.Event.Location));

            CreateMap<Wishlist, WishlistByEventDTO>()
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ReverseMap();



        }
    }
}
