using Common.Database.PO;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace Common.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Executor> Executors { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Scheduler> Schedulers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var time = new DateTime(2023,9,11);
            var defaultUser = new User
            {
                Id = 1,
                Name = "DefaultAccount",
                Pwd = "DefaultPassword",
                Email = "fake@localhost",
                CreateTime = time,
                UpdateTime = time
            };
            modelBuilder.Entity<User>().HasData(defaultUser);

            var MockedExecutor = new Executor
            {
                Id = 1,
                Name = "MockedExecutor",
                LastHeartbeat = time,
                Status = ExecutorStatus.online,
                CreateTime = time,
                UpdateTime = time
            };
            modelBuilder.Entity<Executor>().HasData(MockedExecutor);
            var defaultPackage = new Package
            {
                Id = 1,
                Name = "HelloWorld",
                Version = "1.0.0",
                Description = "Echo helloworld",
                OwnerId = defaultUser.Id,
                StorageAccount = "MoeckedStorageAccount",
                RelativePath = "MoeckedRelativePath",
                CreateTime = time,
                UpdateTime = time
            };

            modelBuilder.Entity<Package>().HasData(defaultPackage);

            var testScheduler = new Scheduler
            {
                Id = 1,
                Name = "TestScheduler",
                Type = SchedulerType.Cron,
                PackageId = defaultPackage.Id,
                AssemblyName = "JobExample",
                Namespace = "JobExample",
                ClassName = "HelloWorldJob",
                ExecutionPlan = "0 0 12 0 0",
                NextExecutionTime = time,
                CreateTime = time,
                UpdateTime = time,
                EndTime = DateTime.MaxValue
            }; 
            modelBuilder.Entity<Scheduler>().HasData(testScheduler);

            var testJob = new Job
            {
                Id = 1,
                Name = "TestJob",
                SchedulerId = testScheduler.Id,
                Status = JobStatus.WaitForExectution,
                ScheduledExecutionTime = time,
                CreateTime = time,
                UpdateTime = time
            };
            modelBuilder.Entity<Job>().HasData(testJob);
        }

    }
}
