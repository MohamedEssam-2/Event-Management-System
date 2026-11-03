using Business_Logic_Layer.Service.Interface;
using Hangfire;

namespace Presentation_Logic_Layer.Background
{
    public class ReminderJob(IReminderService _reminderService)
    {
        // Prevents two sweeps from overlapping (and double-sending) if one runs long.
        [DisableConcurrentExecution(timeoutInSeconds: 300)]
        [AutomaticRetry(Attempts = 2)]
        public async Task RunAsync()
        {
            await _reminderService.SendUpcomingEventRemindersAsync();
        }
    }
}
