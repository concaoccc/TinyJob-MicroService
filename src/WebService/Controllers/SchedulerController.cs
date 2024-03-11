using Common.Database;
using Common.Database.PO;
using Common.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebService.Models;

namespace WebService.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class SchedulerController : ControllerBase
{

    private readonly ILogger<SchedulerController> logger;
    private ISchedulerRepository schedulerRepository;
    private IPackageRepository packageRepository;
    private IUserRepository userRepository;

    private ContentResult _NotFound = new ContentResult
    {
        StatusCode = 400,
        Content = "Scheduler not found",
        ContentType = "text/plain"
    };


    public SchedulerController(ILogger<SchedulerController> logger, ISchedulerRepository schedulerRepository, IPackageRepository packageRepository, IUserRepository userRepository)
    {
        this.logger = logger;
        this.schedulerRepository = schedulerRepository;
        this.packageRepository = packageRepository;
        this.userRepository = userRepository;
    }

    [HttpGet]
    public ActionResult<Scheduler> GetById([System.Web.Http.FromUri] long id)
    {
        var schedulers = schedulerRepository.GetById(id);
        if (schedulers == null)
        {
            return _NotFound;
        }
        return Ok(schedulers);
    }

    [HttpGet("PackageId")]
    public ActionResult<List<Scheduler>> GetByPackage([System.Web.Http.FromUri] long packageId)
    {
        var schedulers = schedulerRepository.GetByPackage(packageId);
        if (schedulers.Count == 0)
        {
            return _NotFound;
        }
        return schedulers;
    }

    [HttpGet("PackageName")]
    public ActionResult<List<Scheduler>> GetByPackage([System.Web.Http.FromUri] string name)
    {
        var owner = this.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        var ownerId = userRepository.GetByUserName(owner).Id;
        List<Scheduler> schedulers = new List<Scheduler>();
        var packages = packageRepository.GetByName(name, ownerId);
        foreach (var package in packages)
        {
            var scheduler = schedulerRepository.GetByPackage(package.Id);
            if(scheduler != null)
            {
                schedulers.AddRange(scheduler);
            }
        }

        if (schedulers.Count == 0)
        {
            return _NotFound;
        }
        return schedulers;
    }


    [HttpGet]
    [Route("Owner")]
    public ActionResult<List<Scheduler>> GetByOwner()
    {
        var owner = this.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        var ownerId = userRepository.GetByUserName(owner).Id;
        var schedulers = schedulerRepository.GeyByOwner(ownerId);
        if (schedulers.Count == 0)
        {
            return _NotFound;
        }
        return schedulers;
    }

    [HttpPost]
    [Route("Create")]
    public ActionResult Create([FromBody] Schedulers schedulers)
    {
        var owner = this.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        var ownerId = userRepository.GetByUserName(owner).Id;
        Package? package = packageRepository.GetByNameAndVersion(schedulers.PackageName, schedulers.PackageVersion, ownerId);
        if (package == null)
        {
            return new ContentResult
            {
                StatusCode = 403,
                Content = "Package not found",
                ContentType = "text/plain"
            };
        }

        var isSuccessful = Enum.TryParse(schedulers.Type, out SchedulerType schedulerType1);
        if (!isSuccessful)
        {
            return new ContentResult
            {
                StatusCode = 403,
                Content = "SchedulerType is not supported",
                ContentType = "text/plain"
            };
        }

        Scheduler scheduler = new Scheduler()
        {
            Name = schedulers.Name,
            Type = schedulerType1,
            PackageId = package.Id,
            AssemblyName = schedulers.AssemblyName,
            Namespace = schedulers.Namespace,
            ClassName = schedulers.ClassName,
            ExecutionPlan = schedulers.ExecutionPlan,
            EndTime = schedulers.EndTime,
            CreateTime = DateTime.UtcNow,
            UpdateTime = DateTime.UtcNow
        };

        bool result= schedulerRepository.Create(scheduler, out string message);
        if (result)
        {
            return Ok(scheduler);
        }
        else
        {
            return new ContentResult
            {
                StatusCode = 409,
                Content = message,
                ContentType = "text/plain"
            };
        }
    }

    [HttpPost("Stop")]
    public ActionResult StopScheduler([System.Web.Http.FromUri] long schedulerId)
    {
        var scheduler = schedulerRepository.GetById(schedulerId);
        if(scheduler == null)
        {
            return _NotFound;
        }

        scheduler.EndTime = DateTime.Now;
        bool result = schedulerRepository.Update(scheduler, out string message);
        if (result)
        {
            return Ok(scheduler);
        }
        else
        {
            return new ContentResult
            {
                StatusCode = 400,
                Content = message,
                ContentType = "text/plain"
            };
        }
    }
}
