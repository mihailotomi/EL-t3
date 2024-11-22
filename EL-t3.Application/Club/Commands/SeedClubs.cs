using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Common.Interfaces.Gateway;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EL_t3.Application.Club.Commands;

public class SeedClubs
{
    public record Command() : IRequest<IEnumerable<string>>;

    public record CommandHandler : IRequestHandler<Command, IEnumerable<string>>
    {
        private readonly IClubGateway _clubGateway;
        private readonly IAppDatabaseContext _dbContext;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(IClubGateway clubGateway, IAppDatabaseContext dbContext, ILogger<CommandHandler> logger)
        {
            _clubGateway = clubGateway;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            List<string> allErrors = [];
            IEnumerable<int> seasons = Enumerable.Range(2000, 2023 - 2000 + 1);
            foreach (int season in seasons)
            {
                _logger.LogInformation("Seeding clubs for season {season}", season);
                var (clubs, seasonErrors) = await _clubGateway.FetchClubsBySeasonAsync(season);
                if (seasonErrors.Any())
                {
                    allErrors.AddRange(seasonErrors);
                    _logger.LogError("There are errors for season {season}", season);
                }

                await _dbContext.Clubs.UpsertRange(clubs)
                    .On((c) => new { c.Code })
                    .WhenMatched((cDb, cIns) => new Domain.Entities.Club()
                    {
                        Name = cDb.Name,
                        Code = cDb.Code,
                        CrestUrl = cDb.CrestUrl ?? cIns.CrestUrl
                    })
                    .RunAsync(cancellationToken);

                _logger.LogInformation("Finished seeding clubs for season {season}", season);
            }

            return allErrors;
        }
    }
}
