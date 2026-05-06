using Microsoft.AspNetCore.Mvc;
using SkyRoute.Api.Common;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.UseCases.SearchFlights;

namespace SkyRoute.Api.Controllers;

[ApiController]
[Route("api/flights")]
public class FlightsController : ControllerBase
{
    private readonly SearchFlightsUseCase _useCase;

    public FlightsController(SearchFlightsUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost("search")]
    public async Task<ActionResult<SearchFlightsResponse>> Search(
        [FromBody] SearchFlightsRequest request,
        CancellationToken ct)
    {
        var result = await _useCase.ExecuteAsync(request, ct);
        return result.ToActionResult(this);
    }
}
