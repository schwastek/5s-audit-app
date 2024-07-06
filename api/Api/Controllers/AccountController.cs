using Api.Contracts.Identity.Dto;
using Api.Contracts.Identity.Requests;
using Core.TokenService;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers;

[ApiController]
[Route("api/account")]
[Produces(MediaTypeNames.Application.Json)]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly TokenService _tokenService;
    private readonly IValidator<RegisterRequest> _registerRequestValidator;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        TokenService tokenService,
        IValidator<RegisterRequest> registerRequestValidator
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _registerRequestValidator = registerRequestValidator;
    }

    /// <summary>
    /// Returns access token
    /// </summary>
    /// <param name="request"></param>
    /// <returns>User information with access token</returns>
    /// <response code="200">User is logged in</response>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> Login(LoginRequest request)
    {
        User user = await _userManager.FindByEmailAsync(request.Email);

        // TODO: Return proper error message.

        if (user == null) return Unauthorized();

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (result.Succeeded)
        {
            await SetRefreshToken(user);
            UserDto userDto = CreateUserObject(user);

            return Ok(userDto);
        }

        return Unauthorized();
    }

    /// <summary>
    /// Registers new user
    /// </summary>
    /// <param name="request"></param>
    /// <returns>New user information with access token</returns>
    /// <response code="200">New user is registered</response>
    /// <response code="400">Validation error</response>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Register(RegisterRequest request)
    {
        _registerRequestValidator.ValidateAndThrow(request);

        if (await _userManager.Users.AnyAsync(x => x.Email == request.Email))
        {
            ModelState.AddModelError("email", "Email taken");
            return ValidationProblem();
        }
        if (await _userManager.Users.AnyAsync(x => x.UserName == request.Username))
        {
            ModelState.AddModelError("username", "Username taken");
            return ValidationProblem();
        }

        User user = new()
        {
            DisplayName = request.DisplayName,
            Email = request.Email,
            UserName = request.Username
        };

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await SetRefreshToken(user);
            UserDto userDto = CreateUserObject(user);

            return Ok(userDto);
        }

        return BadRequest("Problem registering user");
    }

    /// <summary>
    /// Gets user information
    /// </summary>
    /// <returns>User information</returns>
    /// <response code="200">User information</response>
    [HttpGet]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        User user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
        await SetRefreshToken(user);
        UserDto userDto = CreateUserObject(user);

        return Ok(userDto);
    }

    /// <summary>
    /// Refreshes access token
    /// </summary>
    /// <returns>User information with new access token</returns>
    /// <response code="200">User information with new access token</response>
    [HttpPost("refreshToken")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var user = await _userManager.Users
            .Include(r => r.RefreshTokens)
            .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

        if (user == null) return Unauthorized();

        var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

        if (oldToken != null && !oldToken.IsActive)
        {
            return Unauthorized();
        }

        // Generate new JWT
        UserDto userDto = CreateUserObject(user);

        return Ok(userDto);
    }

    private async Task SetRefreshToken(User user)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);
        await _userManager.UpdateAsync(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }

    private UserDto CreateUserObject(User user)
    {
        return new UserDto
        {
            DisplayName = user.DisplayName,
            Token = _tokenService.CreateToken(user),
            Username = user.UserName
        };
    }
}
