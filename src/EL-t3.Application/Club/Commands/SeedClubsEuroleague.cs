using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Common.Interfaces.Gateway;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EL_t3.Application.Club.Commands;

public class SeedClubsEuroleague
{
    public record Command() : IRequest<IEnumerable<string>>;

    public record CommandHandler : IRequestHandler<Command, IEnumerable<string>>
    {
        private readonly IClubBySeasonGateway _clubGateway;
        private readonly IAppDatabaseContext _dbContext;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(IClubBySeasonGateway clubGateway, IAppDatabaseContext dbContext, ILogger<CommandHandler> logger)
        {
            _clubGateway = clubGateway;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            List<string> allErrors = [];
            for (int season = 2024; season >= 2000; season--)
            {
                _logger.LogInformation("Seeding clubs for season {season}", season);
                var (payloads, seasonErrors) = await _clubGateway.FetchClubsBySeasonAsync(season);
                if (seasonErrors.Any())
                {
                    allErrors.AddRange(seasonErrors);
                    _logger.LogError("There are errors for season {season}", season);
                }


                var clubs = payloads.Select(p => Domain.Entities.Club.Create(p.Code, p.Name, p.CrestUrl));

                await _dbContext.Clubs.UpsertRange(clubs)
                    .On((c) => new { c.Code })
                    .WhenMatched(Domain.Entities.Club.Upserter)
                    .RunAsync(cancellationToken);

                _logger.LogInformation("Finished seeding clubs for season {season}", season);
            }

            return allErrors;
        }
    }
}
