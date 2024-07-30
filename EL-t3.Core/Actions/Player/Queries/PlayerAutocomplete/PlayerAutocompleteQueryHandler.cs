using EL_t3.Core.Interfaces.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace EL_t3.Core.Actions.Player.Queries.PlayerAutocomplete;

public record PlayerAutocompleteQueryHandler : IRequestHandler<PlayerAutocompleteQuery, IEnumerable<Entities.Player>>
{
    private readonly IAppDatabaseContext _context;

    public PlayerAutocompleteQueryHandler(IAppDatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Entities.Player>> Handle(PlayerAutocompleteQuery request, CancellationToken cancellationToken)
    {
        var searchPattern = $"%{request.Search.ToUpper()}%";

        return await _context.Players
            .Where(p => EF.Functions.Like(p.FirstName + " " + p.LastName, searchPattern) ||
                    EF.Functions.Like(p.LastName + " " + p.FirstName, searchPattern))
            .ToListAsync(cancellationToken);
    }
} 