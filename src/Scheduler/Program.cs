namespace Scheduler
{
    using Common.Database;
    using Common.Database.Repositories;
    using Common.MessageQueue.Producer;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
                    services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration["DbConnectionString"]));
                    services.AddSingleton<IMqProducer, MqProducer>();
                    services.AddScoped<IJobRepository, JobRepository>();
                    services.AddScoped<IExecutorRepository, ExecutorRepository>();
                    services.AddScoped<IPackageRepository, PackageRepository>();
                    services.AddScoped<ISchedulerRepository, SchedulerRepository>();
                    services.AddHostedService<Worker>();
                });
    }
}