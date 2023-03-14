using Ionta.OSC.App.Services.Scheduler;
using Ionta.OSC.Core.Data;
using Ionta.OSC.Core.ServiceTools;
using Ionta.OSC.ToolKit.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.Scheduler
{
    public class Worker : BackgroundService
    {
        private readonly IServiceManager _serviceManager;
        private readonly IScheduler _scheduler;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public Worker(IServiceManager serviceManager, IScheduler scheduler, IServiceProvider serviceProvider) 
        {
            _serviceManager = serviceManager;
            _scheduler = scheduler;
            _serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scoped = _serviceProvider.CreateScope())
                {
                    var _store = scoped.ServiceProvider.GetRequiredService<IOscStorage>();
                    var jobs = _store.Jobs.Where(job => job.NextExecute <= DateTime.UtcNow);
                    foreach (var job in jobs)
                    {
                        var currentJob = _scheduler.GetJob(job.Name);
                        if (currentJob == null) continue;
                        ExecuteJob(currentJob);
                        job.NextExecute = GetCron(currentJob);
                    }
                    await _store.SaveChangesAsync();
                }
                await Task.Delay(10 * 1000, stoppingToken);
            }
        }

        private DateTime GetCron(JobInfo job)
        {
            switch(job.IntervalType) 
            {
                case JobInterval.Milis: return DateTime.UtcNow.AddMilliseconds(job.Interval);
                case JobInterval.Second: return DateTime.UtcNow.AddSeconds(job.Interval);
                case JobInterval.Min: return DateTime.UtcNow.AddMinutes(job.Interval);
                case JobInterval.Hour: return DateTime.UtcNow.AddHours(job.Interval);
                case JobInterval.Day: return DateTime.UtcNow.AddDays(job.Interval);
            }

            throw new Exception("Cron not found");
        }

        private void ExecuteJob(JobInfo job)
        {
            var args = GetServices(job.Job);
            var instance = (IJob)Activator.CreateInstance(job.Job, args:args);
            Task.Run(instance.Execute);
        }

        private object[] GetServices(Type type)
        {
            var constructorInfo = type.GetConstructors().First();

            var services = constructorInfo.GetParameters()
                .Select(p => _serviceManager.GetService(p.ParameterType, type.Assembly))
                .ToArray();

            return services;
        }
    }
}
