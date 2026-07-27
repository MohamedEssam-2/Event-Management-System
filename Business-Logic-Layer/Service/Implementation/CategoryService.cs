using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.CategoryDTO;
using Business_Logic_Layer.Exceptions.CategoryExceptions;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Data_Access_Layer.Specifications.EventSpecifications;

namespace Business_Logic_Layer.Service.Implementation
{
    public class CategoryService(IUnitOfWork _unitOfWork, IMapper _mapper , ICurrentUserService _currentUser) : ICategoryService
    {
        public async Task<ServiceResponse<int>> CreateCategory(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            category.CreatedBy = _currentUser.FullName;
            category.CreatedAt = DateTime.UtcNow;
            var CategoryRepo = await _unitOfWork.GetRepository<Category, int>().Create(category);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<int>
            {
                Data = CategoryRepo.Id,
                Success = true,
                Message = "Category created successfully"
            };
        }

        public async Task<ServiceResponse<bool>> DeleteCategory(int id)
        {
            var category =await _unitOfWork.GetRepository<Category, int>().GetById(id);
            if (category == null)
                throw new CategoryNotFoundException(id);
         
                 _unitOfWork.GetRepository<Category, int>().Delete(category);
                category.DeletedBy = _currentUser.FullName;
                category.DeletedDate = DateTime.UtcNow;
                await _unitOfWork.SaveChangesAsync();
                return new ServiceResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Category Deleted Successfully"
                };
            
        }

        public async Task<ServiceResponse<List<ReadCategoryDTO>>> GetAllCategories()
        {
            
            var category=await _unitOfWork.GetRepository<Category, int>().GetAll();
            if (category == null)
            {
                return new ServiceResponse<List<ReadCategoryDTO>>
                {
                    Success= false,
                    Message = "Cant Find Any Category ."
                };
            }
                var categoryDTO = _mapper.Map<List<ReadCategoryDTO>>(category);
                return new ServiceResponse<List<ReadCategoryDTO>>
                {
                    Success = true,
                    Data = categoryDTO,
                    Message = "Category Found Successfully"
                };
        }


        public async Task<ServiceResponse<DetailsCategoryDTO>> GetById(int id)
        {
            var category = await _unitOfWork.GetRepository<Category, int>().GetById(id);
            if (category == null)
                throw new CategoryNotFoundException(id);
          
                var categoryDTO = _mapper.Map<DetailsCategoryDTO>(category);
                return new ServiceResponse<DetailsCategoryDTO>
                {
                    Success = true,
                    Data = categoryDTO,
                    Message = "Category Found Successfully"
                };
            
        }

        public async Task<ServiceResponse<bool>> UpdateCat(int id, CategoryDTO categoryDTO)
        {
            var category = await _unitOfWork.GetRepository<Category, int>().GetById(id);
            if (category == null)
                throw new CategoryNotFoundException(id);
           
                if(categoryDTO.Name != null)
                {
                    category.Name = categoryDTO.Name;
                }
                category.UpdatedBy = _currentUser.FullName;
                category.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.GetRepository<Category, int>().Update(category);
                await _unitOfWork.SaveChangesAsync();
                return new ServiceResponse<bool>
                {
                    Success = true,
                    Message = "Category Updated Successfully"
                };
            
        }
    }
}
