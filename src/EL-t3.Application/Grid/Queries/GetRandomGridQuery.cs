using EL_t3.Application.Club.DTOs;
using EL_t3.Application.Common.Exceptions;
using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Application.Grid.Queries;


public class GetRandomGrid
{
    public record Query() : IRequest<Domain.Entities.Grid>;

    public record QueryHandler : IRequestHandler<Query, Domain.Entities.Grid>
    {
        private readonly IAppDatabaseContext _context;

        public QueryHandler(IAppDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Grid> Handle(Query request, CancellationToken cancellationToken)
        {
            var (clubAmount, countryAmount) = GetGridAmountByType();
            var clubs = await GetRandomGridClubs(clubAmount);
            var countries = await GetRandomGridCountries(countryAmount);

            var x = new List<GridItem>();
            x.AddRange(clubs.Select(c => new GridItem
            {
                Type = GridItemType.CLUB,
                Item = c
            }));
            x.AddRange(countries.Select(c => new GridItem
            {
                Type = GridItemType.COUNTRY,
                Item = c
            }));

            var y = (await GetConstraintedClubs(x)).Select(c => new GridItem { Type = GridItemType.CLUB, Item = c });

            return new Domain.Entities.Grid
            {
                X = x,
                Y = y
            };
        }

        private async Task<IList<ClubDTO>> GetRandomGridClubs(int amount)
        {
            var topClubs = await (from ps1 in _context.PlayerSeasons
                                  join ps2 in _context.PlayerSeasons
                                  on ps1.PlayerId equals ps2.PlayerId
                                  let club = ps1.Club
                                  group club by new
                                  {
                                      club.Id,
                                      club.Name,
                                      club.Code,
                                      club.CrestUrl,
                                      club.CreatedAt,
                                      club.UpdatedAt
                                  } into g
                                  orderby g.Count() descending
                                  select new ClubDTO()
                                  {
                                      Id = g.Key.Id,
                                      Name = g.Key.Name,
                                      Code = g.Key.Code,
                                      CrestUrl = g.Key.CrestUrl,
                                      CreatedAt = g.Key.CreatedAt,
                                      UpdatedAt = g.Key.UpdatedAt
                                  }).Take(15).ToListAsync();

            return (from club in topClubs
                    orderby Guid.NewGuid()
                    select club).Take(amount).ToList();
        }

        private async Task<IList<string>> GetRandomGridCountries(int amount)
        {
            if (amount == 0)
            {
                return [];
            }

            var topCountries = await (from ps in _context.PlayerSeasons
                                      where ps.Player != null && ps.Player.Country != null
                                      group ps by ps.Player!.Country into g
                                      orderby g.Count() descending
                                      select g.Key).Take(10).ToListAsync();

            return (from country in topCountries
                    orderby Guid.NewGuid()
                    select country).Take(amount).ToList();
        }

        private async Task<IEnumerable<Domain.Entities.Club>> GetConstraintedClubs(IList<GridItem> constraints)
        {
            if (constraints.Count < 3)
            {
                throw new ArgumentException("Insufficient amount of constraints");
            }

            var sq1 = MapConstraintToSubquery(constraints.ElementAt(0));
            var sq2 = MapConstraintToSubquery(constraints.ElementAt(1));
            var sq3 = MapConstraintToSubquery(constraints.ElementAt(2));

            var query = from c in _context.Clubs
                        join c1 in sq1
                        on c.Id equals c1.ClubId
                        join c2 in sq2
                        on c.Id equals c2.ClubId
                        join c3 in sq3
                        on c.Id equals c3.ClubId
                        orderby Math.Min(c3.Num, Math.Min(c1.Num, c2.Num)) descending
                        select c;

            var topConstraintedClubs = await query.Take(10).ToListAsync();

            return (from club in topConstraintedClubs
                    orderby Guid.NewGuid()
                    select club).Take(3).ToList();
        }

        private IQueryable<ClubCommonPlayersPayload> MapConstraintToSubquery(GridItem constraint)
        {
            switch (constraint.Type)
            {
                case GridItemType.CLUB:
                    return ClubConstraintSq((constraint.Item as Domain.Entities.Club)!.Id);
                case GridItemType.COUNTRY:
                    return CountryConstraintSq((constraint.Item as string)!);
                default:
                    throw new ValidationException("Type", "Grid item must have a valid type.");
            }
        }

        private IQueryable<ClubCommonPlayersPayload> ClubConstraintSq(long clubId)
        {
            return from ps1 in _context.PlayerSeasons
                   join ps2 in _context.PlayerSeasons
                   on ps1.PlayerId equals ps2.PlayerId
                   where ps2.ClubId == clubId
                   where ps1.ClubId != ps2.ClubId
                   group ps1 by ps1.ClubId into g
                   select new ClubCommonPlayersPayload
                   {
                       ClubId = g.Key,
                       Num = g.Count()
                   };
        }

        private IQueryable<ClubCommonPlayersPayload> CountryConstraintSq(string countryCode)
        {
            return from ps in _context.PlayerSeasons
                   join player in _context.Players
                   on ps.PlayerId equals player.Id
                   where player != null && player.Country == countryCode
                   group ps by ps.ClubId into g
                   select new ClubCommonPlayersPayload()
                   {
                       ClubId = g.Key,
                       Num = g.Count()
                   };
        }


        private static (int clubAmount, int countryAmount) GetGridAmountByType()
        {
            var rnd = new Random();
            var rndValue = rnd.NextDouble();

            if (rndValue < 0.25)
            {
                return (clubAmount: 1, countryAmount: 2);
            }
            if (rndValue < 0.5)
            {
                return (clubAmount: 2, countryAmount: 1);
            }
            return (clubAmount: 3, countryAmount: 0);
        }

        private class ClubCommonPlayersPayload
        {
            public long ClubId { get; set; }
            public int Num { get; set; }
        };
    }

}