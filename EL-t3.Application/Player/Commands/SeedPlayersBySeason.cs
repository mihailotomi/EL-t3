using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Common.Interfaces.Gateway;
using EL_t3.Application.Player.Helpers;
using EL_t3.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EL_t3.Application.Player.Commands;

public class SeedPlayersBySeason
{
    public record Command() : IRequest<IEnumerable<string>>;

    public record CommandHandler : IRequestHandler<Command, IEnumerable<string>>
    {
        private readonly IPlayerBySeasonGateway _playerBySeasonGateway;
        private readonly IAppDatabaseContext _dbContext;
        private readonly ILogger<CommandHandler> _logger;
        private readonly PlayerSeedHelper _playerSeedHelper;

        public CommandHandler(IPlayerBySeasonGateway playerBySeasonGateway, IAppDatabaseContext dbContext, ILogger<CommandHandler> logger, PlayerSeedHelper playerSeedHelper)
        {
            _playerBySeasonGateway = playerBySeasonGateway;
            _dbContext = dbContext;
            _logger = logger;
            _playerSeedHelper = playerSeedHelper;
        }
        public async Task<IEnumerable<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            List<string> allErrors = [];

            for (int season = 2000; season <= 2023; season++)
            {
                _logger.LogInformation("Seeding players for season {season}", season);

                var (rawPlayerSeasons, seasonErrors) = await _playerBySeasonGateway.FetchPlayerSeasonsBySeasonAsync(season);
                if (seasonErrors.Any())
                {
                    allErrors.AddRange(seasonErrors);
                }

                var rawPlayers = PlayerSeedHelper.PrepeareUniquePlayersFromPlayerSeasons(rawPlayerSeasons);

                var players = rawPlayers.Select(rp => Domain.Entities.Player.Create(rp.FirstName, rp.LastName, rp.BirthDate, rp.Country, rp.ImageUrl));

                await _dbContext.Players.UpsertRange(players)
                    .On((p) => new { p.FirstName, p.LastName, p.BirthDate })
                    .WhenMatched(Domain.Entities.Player.Upserter)
                    .RunAsync(cancellationToken);

                var preparedPlayerSeasons = await _playerSeedHelper.PreparePlayerSeasons(rawPlayerSeasons, cancellationToken);

                await _dbContext.PlayerSeasons.UpsertRange(preparedPlayerSeasons)
                    .On((p) => new { p.PlayerId, p.ClubId, p.Season })
                    .WhenMatched(PlayerSeason.Upserter)
                    .RunAsync(cancellationToken);

                _logger.LogInformation("Finished seeding players for season {season}", season);

            }

            return allErrors;
        }
    }
}

