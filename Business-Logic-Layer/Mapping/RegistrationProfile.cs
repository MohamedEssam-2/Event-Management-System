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
          CreateMap<Registration, ReadAllRegistrationDTO>().ReverseMap();
        }
    }
}
