using EL_t3.Application.Club.Payloads;
using EL_t3.Infrastructure.Gateway.Contracts;

namespace EL_t3.Infrastructure.Gateway.Extensions;

public static class GatewayClubMappingExtensions
{
    public static CreateClubPayload ToPayload(this GatewayClub gc)
    {
        string crestUrl = gc?.Images?.Crest ?? throw new ArgumentNullException(nameof(gc.Images.Crest), $"No crest found for club {gc!.Code}");

        return new CreateClubPayload
        (
            Code: gc.Code,
            Name: gc.Name,
            CrestUrl: crestUrl
        );
    }
}