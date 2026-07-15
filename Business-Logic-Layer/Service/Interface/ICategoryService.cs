using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.CategoryDTO;

namespace Business_Logic_Layer.Service.Interface
{
    public interface ICategoryService
    {
        public Task<ServiceResponse<List<ReadCategoryDTO>>> GetAllCategories();
        public Task<ServiceResponse<DetailsCategoryDTO>> GetById(int id);   
        public Task<ServiceResponse<bool>> DeleteCategory(int id);
        public Task<ServiceResponse<int>> CreateCategory(CategoryDTO categoryDTO);
        public Task<ServiceResponse<bool>> UpdateCat(int id ,CategoryDTO categoryDTO);
    }
}
