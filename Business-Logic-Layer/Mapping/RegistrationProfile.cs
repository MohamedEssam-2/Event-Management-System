using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.RegistrationDTO;
using Data_Access_Layer.Models;

namespace Business_Logic_Layer.Mapping
{
    public class RegistrationProfile  :Profile
    {
        public RegistrationProfile()
        {
          CreateMap<Registration,RegistrationDTO>().ReverseMap();
          CreateMap<Registration, ReadAllRegistrationDTO>()
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ReverseMap();

        }
    }
}
