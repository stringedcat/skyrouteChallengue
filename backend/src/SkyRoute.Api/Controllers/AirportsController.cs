using Microsoft.AspNetCore.Mvc;
using SkyRoute.Api.Common;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.UseCases.GetAirports;

namespace SkyRoute.Api.Controllers;

[ApiController]
[Route("api/airports")]
public class AirportsController : ControllerBase
{
    private readonly GetAirportsUseCase _useCase;

    public AirportsController(GetAirportsUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AirportDto>>> GetAll(CancellationToken ct)
    {
        var result = await _useCase.ExecuteAsync(ct);
        return result.ToActionResult(this);
    }
}
