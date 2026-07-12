using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Exceptions
{
    public class EventNotFoundException : AppException
    {
        public EventNotFoundException(int id) : base($"Event with ID {id} not found.", (int)HttpStatusCode.NotFound)
        {
        }
    
    }
}
