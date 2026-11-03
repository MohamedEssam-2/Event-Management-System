using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Data_Access_Layer.Specifications.EventSpecifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business_Logic_Layer.Service.Implementation
{
    public class ReminderService(
       IUnitOfWork _unitOfWork,
       INotificationService _notificationService,
       IConfiguration _configuration,
       ILogger<ReminderService> _logger) : IReminderService
    {
        private const int DefaultHoursBeforeEvent = 24;
        private const int BatchSize = 100;
        private const int MaxBatchesPerRun = 50; // safety net: at most 5,000 reminders per run

        public async Task<int> SendUpcomingEventRemindersAsync()
        {
            var hoursBefore = _configuration.GetValue<int?>("ReminderSettings:HoursBeforeEvent") ?? DefaultHoursBeforeEvent;
            if (hoursBefore <= 0)
            {
                hoursBefore = DefaultHoursBeforeEvent;
            }

            var registrationRepo = _unitOfWork.GetRepository<Registration, int>();
            var queued = 0;

            for (var batch = 0; batch < MaxBatchesPerRun; batch++)
            {
                var spec = new ReminderDueRegistrationsSpecification(DateTime.UtcNow, hoursBefore, BatchSize);
                var due = (await registrationRepo.GetAll(spec)).ToList();

                if (due.Count == 0)
                {
                    break;
                }

                foreach (var registration in due)
                {
                    // Queue the email first, then mark as sent. If saving fails the user may get a
                    // duplicate reminder on the next run, which is better than silently losing it.
                    if (!string.IsNullOrWhiteSpace(registration.User.Email))
                    {
                        await _notificationService.EventReminderNotification(
                            registration.User.Email!,
                            registration.User.FullName,
                            registration.Event.Name,
                            registration.Event.Date,
                            registration.Event.Location);
                        queued++;
                    }
                    else
                    {
                        _logger.LogWarning("Registration {RegistrationId} has no user email; skipping reminder.", registration.Id);
                    }

                    // Entities are tracked, so no explicit Update() call is needed (and calling it
                    // would needlessly mark the Event and User rows as modified).
                    registration.ReminderSentAt = DateTime.UtcNow;
                    await _unitOfWork.SaveChangesAsync();
                }
            }

            _logger.LogInformation("Event reminder sweep finished. Reminders queued: {Count}", queued);
            return queued;
        }
    }
}
