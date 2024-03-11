using Common.Database;
using Common.Database.Repositories;
using Common.MessageQueue.Consumer;
using JobExecutor;
using JobExecutor.JobExecutor;
using Microsoft.EntityFrameworkCore;



IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration["DbConnectionString"]));
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddSingleton<IMqConsumer, MqConsumer>();
        services.AddSingleton<IJobHost, JobHost>();
        services.AddScoped<IExecutorRepository, ExecutorRepository>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<ISchedulerRepository, SchedulerRepository>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
