using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberBreakfast.Controllers;
[ApiController]
[Route("[controller]")]
public class ApiControllers : ControllerBase
{
    protected IActionResult Problem(List<Error> errors){
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var modelStateDictionary = new ModelStateDictionary();
            errors.ToList().ForEach(e => modelStateDictionary.AddModelError(e.Code, e.Description));
            return ValidationProblem(modelStateDictionary);
        }

        if (errors.Any(e => e.Type == ErrorType.Unexpected))
        {
            return Problem(
                title: "An unexpected error occurred.",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        var FirstError = errors.First();
        var statusCode = FirstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(
            title: FirstError.Code,
            statusCode: statusCode
        );
    }
}