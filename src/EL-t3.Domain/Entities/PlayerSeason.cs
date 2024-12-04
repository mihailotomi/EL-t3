using System.Linq.Expressions;

namespace EL_t3.Domain.Entities;
public class PlayerSeason : BaseEntity
{
    public long ClubId { get; private set; }
    public Club Club { get; private set; } = null!;
    public long PlayerId { get; private set; }
    public Player Player { get; private set; } = null!;
    public int Season { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }

    private PlayerSeason() { }

    private PlayerSeason(
        long clubId,
        long playerId,
        int season,
        DateOnly startDate,
        DateOnly endDate
        )
    {
        if (endDate <= startDate)
        {
            throw new ArgumentException("endDate must be after start date");
        }

        ClubId = clubId;
        PlayerId = playerId;
        Season = season;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static PlayerSeason Create(
        long clubId,
        long playerId,
        int season,
        DateOnly startDate,
        DateOnly endDate
    )
    {
        return new PlayerSeason(clubId, playerId, season, startDate, endDate);
    }

    public static Expression<Func<PlayerSeason, PlayerSeason, PlayerSeason>> Upserter = (PlayerSeason pDb, PlayerSeason pIns) => new PlayerSeason()
    {
    };
}

