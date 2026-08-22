using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.Service.Interface;

namespace Business_Logic_Layer.Service.Implementation
{
    public class NotificationService(IEmailService _emailService) : INotificationService
    {

        public async Task RegistrationNotification(string userEmail, string userName, string eventName, DateTime eventDate , string eventLocation)
        {
            var subject = "Event Registration Confirmation";

            var body = $@"
                      <h2>Hello {userName}</h2>
                      <p>You have successfully registered for the <b>{eventName} event</b></p>
                      <p>Date: {eventDate:dd/MM/yyyy hh:mm tt}</p>
                      <p>Location: {eventLocation}</p>";

            await _emailService.SendEmailAsync(userEmail, subject, body);
        }
        public async Task PaymentCompletedNotification(string userEmail, string userName, string eventName, DateTime eventDate,string eventLocation, decimal amount)
        {
            var subject = "Payment Completed Successfully";

            var body = $@"
                      <h2>Hello {userName},</h2>
                      <p>Your payment for <b>{eventName}</b> has been completed successfully.</p>
                      <p><b>Date:</b> {eventDate:dd/MM/yyyy hh:mm tt}</p>
                      <p><b>Location:</b> {eventLocation}</p>
                      <p><b>Amount Paid:</b> {amount:C}</p>
                      <p>Your registration has been confirmed.</p>
                      <p>Thank you for your purchase!</p>";

            await _emailService.SendEmailAsync(userEmail,subject,body);
        }
    }


}
