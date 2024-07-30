using EL_t3.Core.Interfaces.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Core.Actions.Player.Queries.CheckConstraints;

public record CheckPlayerConstraintsQueryHandler : IRequestHandler<CheckPlayerConstraintsQuery, bool>
{
    private readonly IAppDatabaseContext _context;

    public CheckPlayerConstraintsQueryHandler(IAppDatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CheckPlayerConstraintsQuery request, CancellationToken cancellationToken)
    {
        foreach (var constraint in request.Constraints)
        {
            var constraintResult = await ValidateConstraint(request.Id, constraint);
            if (!constraintResult)
            {
                return false;
            }
        }
        return true;
    }

    private async Task<bool> ValidateConstraint(int playerId, PlayerConstraint constraint)
    {
        if (constraint is PlayerClubConstraint clubConstraint)
        {
            return await ValidateClubConstraint(playerId, clubConstraint);
        }
        if (constraint is PlayerCountryConstraint countryConstraint)
        {
            return await ValidateCountryConstraint(playerId, countryConstraint);
        }
        return false;
    }

    private async Task<bool> ValidateCountryConstraint(int playerId, PlayerCountryConstraint countryConstraint)
    {
        var num = await _context.Players
                     .Where(p => p.Id == playerId && p.Country == countryConstraint.CountryCode)
                     .CountAsync();

        return num > 0;
    }

    private async Task<bool> ValidateClubConstraint(int playerId, PlayerClubConstraint clubConstraint)
    {
        var num = await _context.PlayerSeasons
                     .Where(ps => ps.ClubId == clubConstraint.Id && ps.PlayerId == playerId)
                     .CountAsync();

        return num > 0;
    }
}
