using Business_Logic_Layer.Service.Interface;
using Hangfire;
namespace Presentation_Logic_Layer.Background
{
    public class BackgroundJobService : IBackgroundJobService
    {
        public void EnqueueEmail(string to, string subject, string message)
        {
            BackgroundJob.Enqueue<IEmailService>(x => x.SendEmailAsync(to,subject,message));
        }
    }
}
