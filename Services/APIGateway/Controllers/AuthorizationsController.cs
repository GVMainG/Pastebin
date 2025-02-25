using APIGateway.Models;
using APIGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pastebin.Infrastructure.SDK.Models;

namespace APIGateway.Controllers;

[Route("api/auth")]
[ApiController]
//[AllowAnonymous]
public class AuthorizationsController : ControllerBase
{
    private readonly UserServices _userServices;

    public AuthorizationsController(UserServices userServices)
    {
        _userServices = userServices;
    }


    [HttpPost("register")]
    public async Task<ActionResult> Registration([FromBody] RegistrationRequest request)
    {
        var result = await _userServices.Registration(request);

        if (result == null || !result.IsRegistered)
        {
            return BadRequest();
        }
        else
        {
            return Created();
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _userServices.Login(request);

        if (result == null || string.IsNullOrEmpty(result.JWTToken))
        {
            return Unauthorized();
        }
        else
        {
            return Accepted(result);
        }
    }
}