using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Exceptions.CategoryExceptions
{
    public class CategoryNotFoundException : AppException
    {
        public CategoryNotFoundException(int id ):base("Category with id " + id + " not found.", (int)HttpStatusCode.NotFound)
        {
            
        }
    }
}
