using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Grid.DTOs;
using EL_t3.Application.Grid.Helpers;
using EL_t3.Application.Grid.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Application.Grid.Queries;


public class GetRandomGrid
{
    public record Query() : IRequest<GridDTO>;

    public record QueryHandler : IRequestHandler<Query, GridDTO>
    {
        private readonly IAppDatabaseContext _context;
        private readonly GridGeneratorService _gridGenerator;

        public QueryHandler(IAppDatabaseContext context, GridGeneratorService gridGenerator)
        {
            _context = context;
            _gridGenerator = gridGenerator;
        }

        public async Task<GridDTO> Handle(Query request, CancellationToken cancellationToken)
        {
            var xTypes = GridConstraintTypeHelper.GetGridItemConstraintsX();
            var xEuroleagueClubAmount = xTypes.Count(x => x == GridItemConstraintType.EUROLEAGUE_CLUB);
            var xCountryAmount = xTypes.Count(x => x == GridItemConstraintType.COUNTRY);

            List<ClubGridItemDTO> xEuroleagueClubs = await _gridGenerator.GetRandomGridClubs(xEuroleagueClubAmount, false);
            List<CountryGridItemDTO> xCounties = await _gridGenerator.GetRandomGridCountries(xCountryAmount);

            var x = new List<GridItemDTO>();
            x.AddRange(xEuroleagueClubs.Select(c => c.ToItemDTO()));
            x.AddRange(xCounties.Select(c => c.ToItemDTO()));

            var yTypes = GridConstraintTypeHelper.GetGridItemConstraintsY();
            var yEuroleagueClubAmount = yTypes.Count(x => x == GridItemConstraintType.EUROLEAGUE_CLUB);
            var yNbaClubAmount = yTypes.Count(x => x == GridItemConstraintType.NBA_CLUB);

            IEnumerable<ClubGridItemDTO> yEuroleagueClubs = await GetConstraintedClubs(x, false, (byte)yEuroleagueClubAmount);
            IEnumerable<ClubGridItemDTO> yNbaClubs = await GetConstraintedClubs(x, true, (byte)yNbaClubAmount);

            var y = new List<GridItemDTO>();
            y.AddRange(yEuroleagueClubs.Select(c => c.ToItemDTO()));
            y.AddRange(yNbaClubs.Select(c => c.ToItemDTO()));

            return new GridDTO(x, y);
        }

        private async Task<IEnumerable<ClubGridItemDTO>> GetConstraintedClubs(List<GridItemDTO> constraints, bool isNba, byte amount)
        {
            if (amount is 0)
            {
                return [];
            }

            if (constraints.Count < 3)
            {
                throw new ArgumentException("Insufficient amount of constraints");
            }

            var sq1 = _gridGenerator.MapConstraintToSubquery(constraints.ElementAt(0));
            var sq2 = _gridGenerator.MapConstraintToSubquery(constraints.ElementAt(1));
            var sq3 = _gridGenerator.MapConstraintToSubquery(constraints.ElementAt(2));

            var query = from c in _context.Clubs
                        where c.IsNba == isNba
                        join c1 in sq1
                        on c.Id equals c1.ClubId
                        join c2 in sq2
                        on c.Id equals c2.ClubId
                        join c3 in sq3
                        on c.Id equals c3.ClubId
                        orderby Math.Min(c3.Num, Math.Min(c1.Num, c2.Num)) descending
                        select new ClubGridItemDTO
                        {
                            Id = c.Id,
                            ImageUrl = c.CrestUrl
                        };


            var topConstraintedClubs = await query.Take(10).ToListAsync();

            return (from club in topConstraintedClubs
                    orderby Guid.NewGuid()
                    select club).Take(amount).ToList();
        }
    }

}