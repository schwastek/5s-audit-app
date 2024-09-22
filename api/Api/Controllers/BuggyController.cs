using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;

namespace Api.Controllers;

[Route("api/errors")]
[ApiController]
[AllowAnonymous]
[Produces(MediaTypeNames.Application.Json)]
public class BuggyController : ControllerBase
{
    /// <response code="404"></response>
    [HttpGet("notfound")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetNotFound()
    {
        return NotFound();
    }

    /// <response code="400"></response>
    [HttpGet("badrequest")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult GetBadRequest()
    {
        return BadRequest();
    }

    /// <response code="500"></response>
    [HttpGet("servererror")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult GetServerError()
    {
        throw new Exception("This is a server error");
    }

    /// <response code="401"></response>
    [HttpGet("unauthorised")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult GetUnauthorised()
    {
        return Unauthorized();
    }
}