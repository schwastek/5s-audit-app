using Api.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [AllowAnonymous]
    public class ErrorController : ControllerBase
    {
        public IActionResult Index(int code)
        {
            var details = new ErrorDetails(code);

            return new ObjectResult(details);
        }
    }
}
