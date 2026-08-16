using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Exceptions
{
    public class BadRequestException : AppException
    {
        public List<string> Errors { get; }

        
        public BadRequestException(string error): base(error, (int)HttpStatusCode.BadRequest)
        {
            Errors = new List<string> { error };
        }

      
        public BadRequestException(List<string> errors): base("Bad Request", (int)HttpStatusCode.BadRequest)
        {
            Errors = errors;
        }
    }
}
