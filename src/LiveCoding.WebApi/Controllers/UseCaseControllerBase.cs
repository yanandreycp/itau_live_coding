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
                return BadRequest(response.Errors);
            }
        }
    }
}