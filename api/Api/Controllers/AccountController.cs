using Api.Exceptions;
using Api.Requests.Identity;
using Api.Requests.Identity.Dto;
using Domain;
using Features.Core.Identity;
using Features.Core.ValidatorService;
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
[Consumes(MediaTypeNames.Application.Json)]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly TokenService _tokenService;
    private readonly IValidatorService _validator;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        TokenService tokenService,
        IValidatorService validator
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _validator = validator;
    }

    /// <summary>
    /// Returns access token
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> Login(LoginRequest request)
    {
        await _validator.ValidateAndThrowAsync(request, HttpContext.RequestAborted);

        var user = await _userManager.FindByEmailAsync(request.Email!);
        if (user is null) return Unauthorized();

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password!, false);

        if (result.Succeeded)
        {
            await SetRefreshToken(user);
            var userDto = CreateUserObject(user);

            return Ok(userDto);
        }

        return Unauthorized();
    }

    /// <summary>
    /// Registers new user
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterRequest request)
    {
        await _validator.ValidateAndThrowAsync(request, HttpContext.RequestAborted);

        var user = new User()
        {
            DisplayName = request.DisplayName,
            Email = request.Email,
            UserName = request.Username
        };

        var result = await _userManager.CreateAsync(user, request.Password!);

        if (result.Succeeded)
        {
            await SetRefreshToken(user);
            var userDto = CreateUserObject(user);

            return Ok(userDto);
        }

        return BadRequest(new CustomValidationProblemDetails()
        {
            Title = "Registration failed",
            Detail = "An error occurred during registration. Please try again later.",
            Instance = HttpContext.Request.Path,
            Errors = result.Errors.Select(e => e.Code).ToArray()
        });
    }

    /// <summary>
    /// Gets user information
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email is null) return Unauthorized();

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return Unauthorized();

        await SetRefreshToken(user);
        var userDto = CreateUserObject(user);

        return Ok(userDto);
    }

    /// <summary>
    /// Refreshes access token
    /// </summary>
    [Authorize]
    [HttpPost("refreshToken")]
    public async Task<ActionResult<UserDto>> RefreshToken()
    {
        var name = User.FindFirstValue(ClaimTypes.Name);
        if (name is null) return Unauthorized();

        var user = await _userManager.Users
            .Include(r => r.RefreshTokens)
            .FirstOrDefaultAsync(x => x.UserName == name);
        if (user is null) return Unauthorized();

        var refreshToken = Request.Cookies["refreshToken"];
        var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);
        if (oldToken is not null && !oldToken.IsActive)
        {
            return Unauthorized();
        }

        // Generate new JWT
        var userDto = CreateUserObject(user);

        return Ok(userDto);
    }

    private async Task SetRefreshToken(User user)
    {
        var refreshToken = TokenService.GenerateRefreshToken();

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
            DisplayName = user.DisplayName!,
            Token = _tokenService.CreateToken(user),
            Username = user.UserName!
        };
    }
}
