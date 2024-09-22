using Api.Exceptions;
using Core.MappingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("errors/{code}")]
[ApiController]
[AllowAnonymous]
// Swagger will ignore this controller
// Note: Don't use `[NonAction]` attribute
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    private readonly IMappingService mapper;

    public ErrorController(IMappingService mapper)
    {
        this.mapper = mapper;
    }

    public IActionResult Index(int code)
    {
        var details = new ErrorDetails(code);
        var response = mapper.Map<ErrorDetails, Contracts.Common.ErrorDetails>(details);

        return new ObjectResult(response);
    }
}
