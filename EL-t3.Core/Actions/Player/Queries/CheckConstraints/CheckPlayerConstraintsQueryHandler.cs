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
        var (countryCode, clubIds) = GetConstraintValuesByType(request.Constraints);
        var query = from ps in _context.PlayerSeasons
                    join p in _context.Players on ps.PlayerId equals p.Id
                    where clubIds.Contains(ps.ClubId)
                          && (string.IsNullOrEmpty(countryCode) || p.Country == countryCode)
                    select ps.ClubId;

        var distinctClubCount = await query.Distinct().CountAsync(cancellationToken: cancellationToken);

        return distinctClubCount == clubIds.Count;
    }

    private (string? country, IList<int> clubIds) GetConstraintValuesByType(IEnumerable<PlayerConstraint> constraints)
    {
        string? country = null;
        var clubIds = new List<int>();

        foreach (var constraint in constraints)
        {
            if (constraint is PlayerClubConstraint clubConstraint)
            {
                clubIds.Add(clubConstraint.ClubId);
            }
            if (constraint is PlayerCountryConstraint countryConstraint)
            {
                country = countryConstraint.CountryCode;
            }
        }

        return (country, clubIds);
    }
}
