using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Exceptions
{
    public class UnauthorizedException(string message): AppException(message, (int)HttpStatusCode.Unauthorized)
    {
    }
    
}
