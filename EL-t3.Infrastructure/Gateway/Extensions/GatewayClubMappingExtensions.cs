using EL_t3.Domain.Entities;
using EL_t3.Infrastructure.Gateway.Contracts;

namespace EL_t3.Infrastructure.Gateway.Extensions;

public static class GatewayClubMappingExtensions
{
    public static Club MapToClubEntity(this GatewayClub gc)
    {
        return new Club
        {
            Name = gc.Name,
            Code = gc.Code,
            CrestUrl = gc?.Images?.Crest
        };
    }
}