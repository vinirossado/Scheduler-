using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using System;

namespace Scheduler.Filters
{
    public class ProlongExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateApplied(
            ApplyStateContext context,
            IWriteOnlyTransaction transaction) =>
            context.JobExpirationTimeout = TimeSpan.FromDays(30);

        public void OnStateUnapplied(
            ApplyStateContext context,
            IWriteOnlyTransaction transaction) =>
            context.JobExpirationTimeout = TimeSpan.FromDays(30);
    }
}
