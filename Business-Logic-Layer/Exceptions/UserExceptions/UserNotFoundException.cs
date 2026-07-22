using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Exceptions.UserExceptions
{
    public class UserNotFoundException  : AppException
    {
        public UserNotFoundException(string id): base($"User with ID {id} not found.", (int)HttpStatusCode.NotFound)
        {
        }
    }
}
