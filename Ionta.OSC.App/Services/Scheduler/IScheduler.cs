
using Ionta.OSC.Core.Data;
using System.Threading.Tasks;

namespace Ionta.OSC.App.Services.Scheduler
{
    public interface IScheduler
    {
        JobInfo GetJob(string name);
        Task AddOrUpdate(JobInfo job, string name);
        Task Remove(string name);
    }
}
