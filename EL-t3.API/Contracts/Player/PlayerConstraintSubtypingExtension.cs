using EL_t3.Application.Common.Exceptions;
using EL_t3.Application.Player.Queries.CheckConstraints;
using EL_t3.Domain.Entities;

namespace EL_t3.API.Contracts.Player;


public static class GatewayPlayerSeasonMappingExtensions
{
    public static PlayerConstraint GetSubtype(this PlayerConstraintDto dto)
    {
        if (dto.Type is null)
        {
            throw new ValidationException("Type", "Invalid value provided.");
        }

        switch (dto.Type)
        {
            case GridItemType.CLUB:
                return new PlayerClubConstraint(dto.Id);
            case GridItemType.COUNTRY:
                return new PlayerCountryConstraint(dto.Code);
            default:
                throw new ValidationException("Type", "Constraint must have a valid type.");
        }
    }
}