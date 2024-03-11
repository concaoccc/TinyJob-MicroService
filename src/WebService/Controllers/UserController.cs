using Common.Database;
using Common.Database.PO;
using Common.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebService.Models;

namespace WebService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly ILogger<SchedulerController> logger;
    private IUserRepository userRepository;


    public UserController(ILogger<SchedulerController> logger, IUserRepository userRepository)
    {
        this.logger = logger;
        this.userRepository = userRepository;
    }

    [HttpPost]
    public ActionResult Create([FromBody] Users users)
    {
        if(string.IsNullOrEmpty(users.UserName) || string.IsNullOrEmpty(users.Password))
        {
            return BadRequest("UserName and Password can not be null");
        }
        MD5 md5 = new MD5CryptoServiceProvider();
        string pwdHash = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(users.Password)));
        User user = new User()
        {
            Name = users.UserName,
            Pwd = pwdHash,
            Email = users.Email,
            CreateTime = DateTime.UtcNow,
            UpdateTime = DateTime.UtcNow
        };

        bool result= userRepository.Create(user, out string message);
        if (result)
        {
            return Ok(user);
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
