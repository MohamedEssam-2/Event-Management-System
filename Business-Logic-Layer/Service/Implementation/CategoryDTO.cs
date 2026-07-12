//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using Business_Logic_Layer.Service.Interface;
//using Data_Access_Layer.Models;
//using Data_Access_Layer.Repository.Interface;

//namespace Business_Logic_Layer.Service.Implementation
//{
//    public class CategoryDTO(IUnitOfWork _unitOfWork , IMapper _mapper) : ICategoryService
//    {
//        public async Task<CategoryDTO> CreateCategory(CategoryDTO categoryDTO)
//        {

//        }

//        public async Task<DTO.CategoryDTO.CategoryDTO> CreateCategory(DTO.CategoryDTO.CategoryDTO categoryDTO)
//        {
//            var entity = _mapper.Map<Category>(categoryDTO);
//            var CategoryRepo = await _unitOfWork.GetRepository<Category, int>().Create(entity);
//            await _unitOfWork.SaveChangesAsync();
//            return _mapper.Map<CategoryDTO>(CategoryRepo);
//        }

//        public Task<bool> DeleteCategory(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<List<DTO.CategoryDTO.CategoryDTO>> GetAllCategories()
//        {
//            throw new NotImplementedException();
//        }

//        public Task<DTO.CategoryDTO.CategoryDTO> GetById(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<DTO.CategoryDTO.CategoryDTO> UpdateCat(DTO.CategoryDTO.CategoryDTO categoryDTO)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
