using Hangfire;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Scheduler.Jobs
{
    public class ExampleJob
    {
        // Voce pode usar injecao de dependencia aqui
        public ExampleJob()
        {
        }

        public async Task ExecuteAsync(IJobCancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await Task.Run(() => Debug.WriteLine("Rodando a job"));
        }
    }
}
