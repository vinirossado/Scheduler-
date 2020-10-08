using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scheduler.Filters;
using Scheduler.Jobs;

namespace Scheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Alterar a sua connection string
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(Configuration.GetConnectionString("Hangfire_ConnectionString")));

            services.AddControllers();

            GlobalJobFilters.Filters.Add(new ProlongExpirationTimeAttribute());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireServer();

            StartJobs();
        }

        private void StartJobs()
        {
            var manager = new RecurringJobManager();

            manager.AddOrUpdate<ExampleJob>(
               "Executa o serviço de exemplo a cada 1 minuto",
               job => job.ExecuteAsync(JobCancellationToken.Null),
               Cron.Minutely);
        }
    }
}
