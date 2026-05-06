using Microsoft.AspNetCore.Mvc;
using SkyRoute.Api.Common;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.UseCases.CreateBooking;
using SkyRoute.Application.UseCases.GetBookingByReference;

namespace SkyRoute.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly CreateBookingUseCase _createUseCase;
    private readonly GetBookingByReferenceUseCase _getUseCase;

    public BookingsController(
        CreateBookingUseCase createUseCase,
        GetBookingByReferenceUseCase getUseCase)
    {
        _createUseCase = createUseCase;
        _getUseCase = getUseCase;
    }

    [HttpPost]
    public async Task<ActionResult<BookingConfirmationDto>> Create(
        [FromBody] CreateBookingRequest request,
        CancellationToken ct)
    {
        var result = await _createUseCase.ExecuteAsync(request, ct);
        return result.ToCreatedResult(this, nameof(GetByReference), new { reference = result.Value?.BookingReference });
    }

    [HttpGet("{reference}")]
    public async Task<ActionResult<BookingConfirmationDto>> GetByReference(
        string reference,
        CancellationToken ct)
    {
        var result = await _getUseCase.ExecuteAsync(reference, ct);
        return result.ToActionResult(this);
    }
}
