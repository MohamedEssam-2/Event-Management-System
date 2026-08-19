using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.OrderDTO;
using Data_Access_Layer.Models;

namespace Business_Logic_Layer.Mapping
{
    partial class OrderProfile : Profile   
    {
        public OrderProfile()
        {
            CreateMap<Order, ReadOrderDTO>()
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ReverseMap();
   
                


        }
    }
}
