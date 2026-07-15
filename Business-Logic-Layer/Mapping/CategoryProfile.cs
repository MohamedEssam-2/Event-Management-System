using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.CategoryDTO;
using Data_Access_Layer.Models;

namespace Business_Logic_Layer.Mapping
{
    public class CategoryProfile  :Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category,CategoryDTO>().ReverseMap();
            CreateMap<Category, ReadCategoryDTO>().ReverseMap();
            CreateMap<Category, DetailsCategoryDTO>().ReverseMap();
        }
    }
}
