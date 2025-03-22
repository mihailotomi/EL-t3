using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Player.Payloads;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Application.Player.Queries
{
    public class FindPlayerByConstraints
    {
        public record Query(IEnumerable<PlayerConstraintPayload> Constraints) : IRequest<IEnumerable<Domain.Entities.Player>>;


        public record QueryHandler : IRequestHandler<Query, IEnumerable<Domain.Entities.Player>>
        {
            private readonly IAppDatabaseContext _dbContext;

            public QueryHandler(IAppDatabaseContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IEnumerable<Domain.Entities.Player>> Handle(Query request, CancellationToken cancellationToken)
            {
                var sq1 = GetConstraintSubquery(request.Constraints.ElementAt(0));
                var sq2 = GetConstraintSubquery(request.Constraints.ElementAt(1));

                return await (from p1 in sq1
                              join p2 in sq2
                              on p1.Id equals p2.Id
                              select p1).ToListAsync(cancellationToken);
            }

            private IQueryable<Domain.Entities.Player> GetConstraintSubquery(PlayerConstraintPayload constraint)
            {
                if (constraint.Type == GridItemType.CLUB)
                {
                    return ClubConstraintSq(long.Parse(constraint.Item));
                }
                return CountryConstraintSq(constraint.Item);
            }

            private IQueryable<Domain.Entities.Player> CountryConstraintSq(string country)
            {
                return _dbContext.Players.Where(p => p.Country == country);
            }

            private IQueryable<Domain.Entities.Player> ClubConstraintSq(long clubId)
            {
                return _dbContext.Players.Where(p => p.SeasonsPlayed.Any(ps => ps.ClubId == clubId));
            }
        }
    }
}
