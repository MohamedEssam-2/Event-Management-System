using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.AccountDTO;
using Data_Access_Layer.Models;

namespace Business_Logic_Layer.Mapping
{
    public class RefreshTokenProfile  : Profile
    {
        public RefreshTokenProfile()
        {
            CreateMap<RefreshToken,RefreshTokenDTO>().ReverseMap();
        }
    }
}
