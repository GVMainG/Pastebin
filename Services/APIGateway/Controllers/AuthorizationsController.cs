using APIGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pastebin.Infrastructure.SDK.Models;

namespace APIGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthorizationsController : ControllerBase
{
    [HttpPost("auth/register")]
    public OkResult Registration([FromBody] RegistrationRequest request)
    {
        return Ok();
    }

    [HttpPost("auth/login")]
    public OkObjectResult Login([FromBody] LoginRequest request)
    {
        var user = new UserModel()
        {
            Id = Guid.NewGuid(),
            Login = request.Login,
            Email = string.Empty,
            RoleId = Guid.NewGuid()
        };

        

        return Ok(user);
    }
}