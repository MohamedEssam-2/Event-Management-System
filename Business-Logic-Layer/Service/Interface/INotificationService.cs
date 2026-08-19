using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.Interface
{
    public interface INotificationService
    {
        public Task RegistrationNotification(string userEmail, string userName, string eventName, DateTime eventDate, string eventLocation);
    }
}
