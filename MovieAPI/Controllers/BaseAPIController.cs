
namespace MovieAPI.Controllers;

public class BaseAPIController : Controller
{
    [NonAction]
    public IActionResult OK(int statusCode, string statusMessage, object? result)
    {
        ResponseModelDto.ResponseModel<object> responseModel = new()
        {
            StatusCode = statusCode,
            StatusMessage = statusMessage,
            Result = result
        };

        return Ok(responseModel);
    }
}

