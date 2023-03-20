using Ionta.OSC.Core.Data;
using Ionta.OSC.ToolKit.Scheduler;

using System.Reflection;

namespace Ionta.OSC.Core.Assemblys.V2.Handlers
{
    public class JobHandler : IGetTypeHandler<JobInfo>
    {
        public Type Type => typeof(JobInfo);

        public IEnumerable<JobInfo> Handle(Assembly assembly)
        {
            var jobs = assembly.GetTypes()
                    .Where(a => typeof(IJob).IsAssignableFrom(a));
            var result = jobs.Where(job => job.GetCustomAttribute<JobIntervalAttribute>() != null).Select(job => {

                var intervalInfo = job.GetCustomAttribute<JobIntervalAttribute>();

                return new JobInfo()
                {
                    Job = job,
                    Interval = intervalInfo!.Interval,
                    IntervalType = intervalInfo!.IntervalType,
                };
            });

            return result;
        }
    }
}
