using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Exceptions
{
    public class EventDeletedException(int id) : Exception($"Event with ID {id} has been deleted.")
    {
    }
}
