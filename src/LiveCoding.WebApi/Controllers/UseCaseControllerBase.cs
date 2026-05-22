using LiveCoding.Application.Generics;
using Microsoft.AspNetCore.Mvc;

namespace LiveCoding.WebApi.Controllers
{
    public class UseCaseControllerBase : ControllerBase
    {
        protected ActionResult GetActionResult<T>(Response<T> response)
        {
            if (response.IsSuccess)
            {
                if (response.Content != null)
                {
                    return Ok(response.Content);
                }
                return NoContent();

            }
            else
            {
                var problemDetails = new ProblemDetails
                {
                    Type = "https://httpstatuses.io/400",
                    Title = "Bad Request",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "One or more validation errors occurred.",
                    Extensions = { { "errors", response.Errors } }
                };
                return BadRequest(problemDetails);
            }
        }
    }
}