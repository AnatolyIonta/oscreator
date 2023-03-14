using Ionta.OSC.Core.Assemblys.V2;
using Ionta.OSC.Core.Data;
using Ionta.OSC.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ionta.OSC.App.Services.Scheduler
{
    public class Scheduler : IScheduler
    {
        private ConcurrentDictionary<string, JobInfo> jobs = new();
        private readonly IAssemblyStore _assemblyStore;
        private readonly IServiceProvider _serviceProvider;
        public Scheduler(IAssemblyStore store, IServiceProvider serviceProvider) 
        { 
            _serviceProvider = serviceProvider;
            _assemblyStore = store;
            _assemblyStore.OnLoad += OnLoad;
            _assemblyStore.OnUnloading += OnUnloading;
        }

        public async Task AddOrUpdate(JobInfo job, string name)
        {
            using (var scoped = _serviceProvider.CreateScope())
            {
                var _store = scoped.ServiceProvider.GetRequiredService<IOscStorage>();
                var jobRow = _store.Jobs.FirstOrDefault(x => x.Name == name);
                if (jobRow != null)
                {
                    var isExist = jobs.TryGetValue(name, out var item);
                    if (isExist) jobs.TryRemove(name, out item);
                }
                else
                {
                    jobRow = new JobInformation() { Name = name, NextExecute = DateTime.UtcNow };
                    _store.Jobs.Add(jobRow);
                }
                var result = jobs.TryAdd(name, job);
                await _store.SaveChangesAsync();
            }
        }

        public async Task Remove(string name)
        {
            using (var scoped = _serviceProvider.CreateScope())
            {
                var _store = scoped.ServiceProvider.GetRequiredService<IOscStorage>();
                var jobRow = _store.Jobs.FirstOrDefault(x => x.Name == name);
                if (jobRow != null)
                {
                    _store.Jobs.Remove(jobRow);
                }

                var isExist = jobs.TryGetValue(name, out var item);
                if (isExist)
                {
                    jobs.TryRemove(name, out item);
                }

                await _store.SaveChangesAsync();
            }
        }

        public JobInfo GetJob(string name)
        {
            jobs.TryGetValue(name, out var item);
            return item;
        }

        private void OnUnloading(Assembly[] obj)
        {
            var jobs = _assemblyStore.Get<JobInfo>(obj);

            foreach (var job in jobs)
            {
                Remove(job.Job.Name + "-" + job.Job.Assembly.FullName);
            }
        }

        private void OnLoad(Assembly[] obj)
        {
            var jobs = _assemblyStore.Get<JobInfo>(obj);

            foreach (var job in jobs)
            {
                AddOrUpdate(job, job.Job.Name + "-" + job.Job.Assembly.FullName);
            }
        }
    }
}
