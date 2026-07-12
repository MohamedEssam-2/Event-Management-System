using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Exceptions
{
    public class EventDeletedException : AppException
    {
        public EventDeletedException(int id):base($"Event with ID {id} has been deleted.", (int)HttpStatusCode.Gone)
        {
            
        }
    }
}
