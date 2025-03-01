using APIGateway.Models;
using APIGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pastebin.Infrastructure.SDK.Models;

namespace APIGateway.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthorizationsController : ControllerBase
{
    private readonly UserServices _userServices;
    private readonly ILogger<AuthorizationsController> _logger;

    public AuthorizationsController(UserServices userServices, ILogger<AuthorizationsController> logger)
    {
        if (userServices == null)
            throw new ArgumentNullException(nameof(userServices));
        if (logger == null)
            throw new ArgumentNullException(nameof(logger));

        _userServices = userServices;
        _logger = logger;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Registration([FromBody] RegistrationRequest request)
    {
        _logger.LogDebug($"{nameof(Registration)}:" + "{@request}", request);

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
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogDebug($"{nameof(Login)}:" + "{@request}", request);

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

    [HttpPut("edit")]
    [Authorize]
    public async Task<IActionResult> UserEditRequest([FromBody] UserEditRequest request)
    {
        _logger.LogDebug($"{nameof(UserEditRequest)}:" + "{@request}", request);

        try
        {
            await _userServices.UserEditRequest(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [HttpDelete("delete/{id}")]
    [Authorize]
    public async Task<IActionResult> UserDeleteRequest(Guid id)
    {
        _logger.LogDebug($"{nameof(UserDeleteRequest)}:" + "{@id}", id);

        try
        {
            await _userServices.UserDeleteRequest(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}