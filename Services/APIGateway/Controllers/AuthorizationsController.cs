using APIGateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationsController : ControllerBase
{
    [HttpGet("auth/register")]
    public OkObjectResult Registration([FromBody] RegistrationRequest request)
    {
        return Ok(Guid.NewGuid());
    }

    [HttpGet("auth/login ")]
    public OkObjectResult Login([FromBody] RegistrationRequest request)
    {
        return Ok(true);
    }
}