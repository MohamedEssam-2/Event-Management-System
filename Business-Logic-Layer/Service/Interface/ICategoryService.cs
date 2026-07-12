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
        public Task<List<CategoryDTO>> GetAllCategories();
        public Task<CategoryDTO> CreateCategory(CategoryDTO categoryDTO);
        public Task<CategoryDTO> GetById(int id);   
        public Task<bool> DeleteCategory(int id);
        public Task<CategoryDTO>UpdateCat(CategoryDTO categoryDTO);
    }
}
