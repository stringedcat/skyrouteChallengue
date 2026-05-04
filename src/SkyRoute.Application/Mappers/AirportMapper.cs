using SkyRoute.Application.DTOs;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Mappers;

public static class AirportMapper
{
    public static AirportDto ToDto(Airport airport)
    {
        return new AirportDto(
            Code: airport.Code,
            Name: airport.Name,
            City: airport.City,
            Country: airport.Country,
            CountryName: airport.CountryName);
    }
}
