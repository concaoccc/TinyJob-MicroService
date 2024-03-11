using Common.Database;
using Common.Database.PO;
using Common.Database.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebService.Models;
using System.Security.Claims;

namespace WebService.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class PackageController : ControllerBase
{

    private readonly ILogger<PackageController> logger;
    private IPackageRepository packageRepository;
    private IUserRepository userRepository;

    private ContentResult _NotFound = new ContentResult
    {
        StatusCode = 400,
        Content = "Package not found",
        ContentType = "text/plain"
    };


    public PackageController(ILogger<PackageController> logger, IPackageRepository packageRepository, IUserRepository userRepository)
    {
        this.logger = logger;
        this.packageRepository = packageRepository;
        this.userRepository = userRepository;
    }

    [HttpGet]
    public ActionResult<Package> GetPackageById([System.Web.Http.FromUri] long id)
    {
        var pkgs = packageRepository.GetById(id);
        if (pkgs == null)
        {
            return _NotFound;
        }
        return pkgs;
    }

    [HttpGet("Name")]
    public ActionResult<List<Package>> GetPackageByName([System.Web.Http.FromUri] string name)
    {
        var owner = this.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        var ownerId = userRepository.GetByUserName(owner).Id;
        var pkgs = packageRepository.GetByName(name, ownerId);
        if (pkgs.Count == 0)
        {
            return _NotFound;
        }
        return pkgs;
    }

    [HttpGet("Name&Version")]
    public ActionResult<Package> GetByNameAndVersion([System.Web.Http.FromUri] string name, [System.Web.Http.FromUri] string version)
    {
        var owner = this.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        var ownerId = userRepository.GetByUserName(owner).Id;
        var pkgs = packageRepository.GetByNameAndVersion(name, version, ownerId);
        if (pkgs == null)
        {
            return _NotFound;
        }
        return pkgs;
    }

    [HttpGet]
    [Route("Owner")]
    public ActionResult<List<Package>> GetPackageByOwner()
    {
        var owner = this.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        var ownerId = userRepository.GetByUserName(owner).Id;
        var pkgs = packageRepository.GetByOwner(ownerId);
        if (pkgs.Count == 0)
        {
            return _NotFound;
        }
        return pkgs;
    }

    [HttpPost]
    public ActionResult Create([FromBody] Pacakges pacakges)
    {
        if(string.IsNullOrEmpty(pacakges.Name) || string.IsNullOrEmpty(pacakges.Version))
        {
            return BadRequest("PackageName and Version can not be null");
        }

        var ownerName = this.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        var owner = userRepository.GetByUserName(ownerName);

        Package pkg = new Package()
        {
            Name = pacakges.Name,
            Version = pacakges.Version,
            Owner = owner,
            StorageAccount = pacakges.StorageAccount,
            RelativePath = pacakges.RelativePath,
            Description = pacakges.Description,
            CreateTime = DateTime.UtcNow,
            UpdateTime = DateTime.UtcNow
        };
        bool result= packageRepository.Create(pkg, out string message);
        if (result)
        {
            return Ok(pkg);
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
}
