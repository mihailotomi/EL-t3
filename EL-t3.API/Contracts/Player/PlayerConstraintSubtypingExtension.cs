using EL_t3.Core.Actions.Player.Queries.CheckConstraints;
using EL_t3.Core.Exceptions;

namespace EL_t3.API.Contracts.Player;


public static class GatewayPlayerSeasonMappingExtensions
{
    public static PlayerConstraint GetSubtype(this PlayerConstraintDto dto)
    {
        if(dto.Type is null){
            throw new ValidationException("Type","Invalid value provided.");
        }

        switch (dto.Type)
        {
            case PlayerConstraintType.CLUB:
                return new PlayerClubConstraint(dto.Id);
            case PlayerConstraintType.COUNTRY:
                return new PlayerCountryConstraint(dto.Code);
            default:
                throw new ValidationException("Type", "Constraint must have a valid type.");
        }
    }
}