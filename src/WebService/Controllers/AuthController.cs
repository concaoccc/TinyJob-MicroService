using Common.Database;
using Common.Database.PO;
using Common.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using WebService.Services;
using WebService.Models;
using System.Security.Authentication;

namespace WebService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationService authenticationService;

    public AuthController(AuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    [HttpPost]
    public ActionResult Authenticate([FromBody] UserCredential userCredential)
    {
        try
        {
            string token = authenticationService.Authenticate(userCredential);
            return Ok(token);
        }
        catch (InvalidCredentialException)
        {
            return Unauthorized();
        }
    }
}
