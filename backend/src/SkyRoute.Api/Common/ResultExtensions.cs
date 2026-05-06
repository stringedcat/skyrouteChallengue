using Microsoft.AspNetCore.Mvc;
using SkyRoute.Application.Common;

namespace SkyRoute.Api.Common;

public static class ResultExtensions
{
    public static ActionResult<T> ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.Ok(result.Value);

        return result.Error!.Type switch
        {
            ErrorType.Validation => controller.BadRequest(new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.NotFound => controller.NotFound(new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.Conflict => controller.Conflict(new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.ProviderFailure => controller.StatusCode(502, new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.Unexpected => controller.StatusCode(500, new { error = result.Error.Code, message = result.Error.Message }),
            _ => controller.StatusCode(500, new { error = "UNKNOWN_ERROR", message = "An unknown error occurred" })
        };
    }

    public static ActionResult<T> ToCreatedResult<T>(this Result<T> result, ControllerBase controller, string actionName, object? routeValues)
    {
        if (result.IsSuccess)
            return controller.CreatedAtAction(actionName, routeValues, result.Value);

        return result.ToActionResult(controller);
    }
}
