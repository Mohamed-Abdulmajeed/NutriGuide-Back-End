using Microsoft.AspNetCore.SignalR;
using NutriGuide.Hubs;
using NutriGuide.Models;
using NutriGuide.UnitOfWorks;
using System;

namespace NutriGuide.Services
{
    public class MedicineReminderService: BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<NotificationHub> _hub;

        public MedicineReminderService(
            IServiceScopeFactory scopeFactory,
            IHubContext<NotificationHub> hub)
        {
            _scopeFactory = scopeFactory;
            _hub = hub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckMedicineTimes();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task CheckMedicineTimes()
        {
            using var scope = _scopeFactory.CreateScope();

            var unit = scope.ServiceProvider
                .GetRequiredService<UnitOfWork>();

            var now = TimeOnly.FromDateTime(DateTime.Now);

            var medicines = unit.MedicineRepo.GetAllIncluding();

            var dueMedicines = medicines
                .SelectMany(m => m.MedicineTimes
                    .Where(mt => mt.TakeTime.Hour == now.Hour &&
                                 mt.TakeTime.Minute == now.Minute)
                    .Select(mt => new { Medicine = m, Time = mt }))
                .ToList();

            foreach (var item in dueMedicines)
            {
                var notification = new Notification
                {
                    Title = "تذكير بالدواء",
                    Message = $"خُد دواء {item.Medicine.MedicineName} ",
                    CustomerId = item.Medicine.CustomerId,
                    ScheduleTime = DateTime.Now,
                    Type = item.Medicine.Option,
                    IsRead = false
                };

                unit.NotificationRepo.Add(notification);
                unit.Save();

                
                await _hub.Clients
                        .Group($"Customer-{item.Medicine.CustomerId}")
                        .SendAsync("MedicineReminder", new
                        {
                            medicineName = item.Medicine.MedicineName,
                            option = item.Medicine.Option,
                            time = item.Time.TakeTime.ToString("HH:mm")
                        });
            }
        }
    }
}
