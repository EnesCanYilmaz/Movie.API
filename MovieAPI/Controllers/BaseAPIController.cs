using System;
using Microsoft.AspNetCore.Mvc;
using static MovieAPI.DTO.ResponseModelDTO;

namespace MovieAPI.Controllers
{
	public class BaseAPIController : Controller
	{
        public IActionResult OK( int statusCode,string statusMessage,object result)
        {
            ResponseModel<object> responseModel = new()
            {
                StatusCode = statusCode,
                StatusMessage = statusMessage,
                Result = result
            };

            return Ok(responseModel);
        }
    }
}

