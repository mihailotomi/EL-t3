using EL_t3.Application.Common.Interfaces.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Application.Player.Queries;

public class FindDisjointPlayers
{
    public record DisjointPlayers(Domain.Entities.Player Player1, Domain.Entities.Player Player2);

    public record Query() : IRequest<IEnumerable<DisjointPlayers>>;


    public record QueryHandler : IRequestHandler<Query, IEnumerable<DisjointPlayers>>
    {
        private readonly IAppDatabaseContext _dbContext;

        public QueryHandler(IAppDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DisjointPlayers>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await DisjointPlayersQuery.ToListAsync(cancellationToken);
        }

        private IQueryable<DisjointPlayers> DisjointPlayersQuery =>
                   from p1 in _dbContext.Players
                   join p2 in _dbContext.Players
                   on new { p1.LastName, p1.BirthDate.Year, p1.BirthDate.Month, FirstName = p1.FirstName.Substring(0, 1) } equals new { p2.LastName, p2.BirthDate.Year, p2.BirthDate.Month, FirstName = p2.FirstName.Substring(0, 1) }
                   where p1.Id > p2.Id // Ensures no duplicate
                   select new DisjointPlayers(p1, p2);
    }


}

