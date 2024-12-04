using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Common.Interfaces.Gateway;
using EL_t3.Application.Player.Helpers;
using EL_t3.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EL_t3.Application.Player.Commands;

public class SeedNbaPlayerSeasonsByClub
{
    public record Command(IEnumerable<string> ClubCodes) : IRequest<IEnumerable<string>>;

    public record CommandHandler : IRequestHandler<Command, IEnumerable<string>>
    {
        private readonly IPlayerByClubGateway _playerByClubGateway;
        private readonly IAppDatabaseContext _dbContext;
        private readonly ILogger<CommandHandler> _logger;
        private readonly PlayerSeedHelper _playerSeedHelper;

        public CommandHandler(IPlayerByClubGateway playerByClubGateway, IAppDatabaseContext dbContext, ILogger<CommandHandler> logger, PlayerSeedHelper playerSeedHelper)
        {
            _playerByClubGateway = playerByClubGateway;
            _dbContext = dbContext;
            _logger = logger;
            _playerSeedHelper = playerSeedHelper;
        }
        public async Task<IEnumerable<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            List<string> allErrors = [];

            foreach (var clubCode in request.ClubCodes)
            {
                _logger.LogInformation("Seeding players for club {club}", clubCode);

                var (rawPlayerSeasons, seasonErrors) = await _playerByClubGateway.FetchPlayersSeasonsByClubAsync(clubCode, isNba: true, cancellationToken);
                if (seasonErrors.Any())
                {
                    allErrors.AddRange(seasonErrors);
                }

                var preparedPlayerSeasons = await _playerSeedHelper.PreparePlayerSeasons(rawPlayerSeasons, cancellationToken);

                await _dbContext.PlayerSeasons
                    .UpsertRange(preparedPlayerSeasons)
                    .On((p) => new { p.PlayerId, p.ClubId, p.Season })
                    .WhenMatched(PlayerSeason.Upserter)
                    .RunAsync(cancellationToken);

                _logger.LogInformation("Finished seeding players for club {club}", clubCode);
            }

            return allErrors;
        }
    }
}

