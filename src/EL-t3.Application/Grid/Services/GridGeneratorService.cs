using EL_t3.Application.Common.Exceptions;
using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Grid.DTOs;
using EL_t3.Application.Grid.Helpers;
using EL_t3.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Application.Grid.Services;

public class ClubCommonPlayersPayload
{
    public long ClubId { get; set; }
    public int Num { get; set; }
};



public class GridGeneratorService
{
    private readonly IAppDatabaseContext _dbContext;
    public GridGeneratorService(IAppDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ClubGridItemDTO>> GetRandomGridClubs(int amount, bool isNba)
    {
        if (amount == 0)
        {
            return [];
        }

        var topClubs = await (from ps1 in _dbContext.PlayerSeasons
                              join ps2 in _dbContext.PlayerSeasons
                              on ps1.PlayerId equals ps2.PlayerId
                              let club = ps1.Club
                              where club.IsNba == isNba
                              group club by new
                              {
                                  club.Id,
                                  club.CrestUrl
                              } into g
                              orderby g.Count() descending
                              select new ClubGridItemDTO
                              {
                                  Id = g.Key.Id,
                                  ImageUrl = g.Key.CrestUrl
                              }).Take(15).ToListAsync();

        return (from club in topClubs
                orderby Guid.NewGuid()
                select club).Take(amount).ToList();
    }

    public async Task<List<CountryGridItemDTO>> GetRandomGridCountries(int amount)
    {
        if (amount == 0)
        {
            return [];
        }

        var topCountries = await (from ps in _dbContext.PlayerSeasons
                                  where ps.Player != null && ps.Player.Country != null
                                  group ps by ps.Player!.Country into g
                                  orderby g.Count() descending
                                  select new CountryGridItemDTO
                                  {
                                      Code = g.Key,
                                      ImageUrl = CountryImageUrlHelper.GetCountryImageUrl(g.Key)
                                  }).Take(10).ToListAsync();

        return (from country in topCountries
                orderby Guid.NewGuid()
                select country).Take(amount).ToList();
    }

    public async Task<List<long>> GetRandomGridTeammates(int amount)
    {
        if (amount == 0)
        {
            return [];
        }

        var topPlayerIds = await (from ps1 in _dbContext.PlayerSeasons
                                  join ps2 in _dbContext.PlayerSeasons
                                  on new { ps1.ClubId, ps1.Season } equals new { ps2.ClubId, ps2.Season }
                                  where ps1.PlayerId != ps2.PlayerId
                                  group ps1 by new
                                  {
                                      ps1.PlayerId
                                  } into g
                                  orderby g.Count() descending
                                  select g.Key.PlayerId).Take(15).ToListAsync();

        return (from playerId in topPlayerIds
                orderby Guid.NewGuid()
                select playerId).Take(amount).ToList();
    }

    public IQueryable<ClubCommonPlayersPayload> MapConstraintToSubquery(GridItemDTO constraint)
    {
        switch (constraint.Type)
        {
            case GridItemType.CLUB:
                return ClubConstraintSq(long.Parse(constraint.Item));
            case GridItemType.COUNTRY:
                return CountryConstraintSq(constraint.Item!);
            case GridItemType.TEAMMATE:
                return TeammateConstraintSq(long.Parse(constraint.Item));
            default:
                throw new ValidationException("Type", "Grid item must have a valid type.");
        }
    }

    private IQueryable<ClubCommonPlayersPayload> CountryConstraintSq(string countryCode)
    {
        return from ps in _dbContext.PlayerSeasons
               where ps.Player != null && ps.Player.Country == countryCode
               group ps by ps.ClubId into g
               select new ClubCommonPlayersPayload()
               {
                   ClubId = g.Key,
                   Num = g.Count()
               };
    }

    private IQueryable<ClubCommonPlayersPayload> ClubConstraintSq(long clubId)
    {
        return from ps1 in _dbContext.PlayerSeasons
               join ps2 in _dbContext.PlayerSeasons
               on ps1.PlayerId equals ps2.PlayerId
               where ps2.ClubId == clubId
               where ps1.ClubId != ps2.ClubId
               group ps1 by ps1.ClubId into g
               select new ClubCommonPlayersPayload()
               {
                   ClubId = g.Key,
                   Num = g.Count()
               };
    }

    private IQueryable<ClubCommonPlayersPayload> TeammateConstraintSq(long playerId)
    {
        return from ps1 in _dbContext.PlayerSeasons
               join ps2 in _dbContext.PlayerSeasons
               on new { ps1.ClubId, ps1.Season } equals new { ps2.ClubId, ps2.Season }
               where ps2.PlayerId == playerId
               where ps1.PlayerId != ps2.PlayerId
               group ps1 by ps1.PlayerId into g
               select new ClubCommonPlayersPayload()
               {
                   ClubId = g.Key,
                   Num = g.Count()
               };
    }
}

