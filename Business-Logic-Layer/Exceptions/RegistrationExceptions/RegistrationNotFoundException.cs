using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Exceptions.RegistrationExceptions
{
    public class RegistrationNotFoundException : AppException   
    {
        public RegistrationNotFoundException(int id ):base("Registration with id " + id + " not found.", (int)System.Net.HttpStatusCode.NotFound)
        {
            
        }
    }
}
