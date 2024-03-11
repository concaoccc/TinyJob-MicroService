using Common.Database;
using Common.Database.PO;
using Common.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Web.Http.Routing;
using System.Security.Claims;

namespace WebService.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class JobController : ControllerBase
{

    private readonly ILogger<JobController> logger;
    private IJobRepository jobRepository;
    private readonly IUserRepository userRepository;

    private ContentResult _NotFound = new ContentResult
    {
        StatusCode = 400,
        Content = "Job not found",
        ContentType = "text/plain"
    };


    public JobController(ILogger<JobController> logger, IJobRepository jobRepository, IUserRepository userRepository)
    {
        this.logger = logger;
        this.jobRepository = jobRepository;
        this.userRepository = userRepository;
    }

    [HttpGet]
    public ActionResult<Job> GetById([System.Web.Http.FromUri] long id)
    {
        var jobs = jobRepository.GetById(id);
        if (jobs == null)
        {
            return _NotFound;
        }
        return jobs;
    }

    [HttpGet]
    [Route("ByScheduler")]
    public ActionResult<List<Job>> GetByScheduler([System.Web.Http.FromUri] long schedulerId)
    {
        var jobs = jobRepository.GetByScheduler(schedulerId);
        if (jobs.Count == 0)
        {
            return _NotFound;
        }
        return jobs;
    }

    [HttpGet]
    [Route("ByOwner")]
    public ActionResult<List<Job>> GetByOwner()
    {
        var owner = this.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        var ownerId = userRepository.GetByUserName(owner).Id;
        var jobs = jobRepository.GetByOwner(ownerId);

        if (jobs.Count == 0)
        {
            return _NotFound;
        }
        return jobs;
    }
}
