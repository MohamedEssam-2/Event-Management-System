using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.EventDTO;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Business_Logic_Layer.Mapping
{
    public class EventProfile:Profile
    {
        public EventProfile()
        {

            CreateMap<Event, ReadAllEventDTO>()
                .ForMember(dest => dest.Category_id, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.Category_name, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl)).ReverseMap();




            CreateMap<CreateEventDTO, Event>();
        }
    }
}
