namespace SkyRoute.Application.DTOs;

public record AirportDto(
    string Code,
    string Name,
    string City,
    string Country,
    string CountryName);
