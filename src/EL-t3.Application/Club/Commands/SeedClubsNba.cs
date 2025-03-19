using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Common.Interfaces.Gateway;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EL_t3.Application.Club.Commands;

public class SeedClubsNba
{
    public record Command() : IRequest<IEnumerable<string>>;

    public record CommandHandler : IRequestHandler<Command, IEnumerable<string>>
    {
        private readonly IAllClubsGateway _clubGateway;
        private readonly IAppDatabaseContext _dbContext;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(IAllClubsGateway clubGateway, IAppDatabaseContext dbContext, ILogger<CommandHandler> logger)
        {
            _clubGateway = clubGateway;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var (payloads, failures) = await _clubGateway.FetchAllClubs(isNba: true);
            if (failures.Any())
            {
                _logger.LogError("There are errors while seeding NBA clubs");
            }

            // add -NBA tag to code because of possible conflict with Euroleague club codes
            var clubs = payloads.Select(p => Domain.Entities.Club.Create(name: p.Name, code: p.Code + "-NBA", p.CrestUrl, isNba: true));

            await _dbContext.Clubs.UpsertRange(clubs)
                .On((c) => new { c.Code })
                .WhenMatched(Domain.Entities.Club.Upserter)
                .RunAsync(cancellationToken);

            return failures;
        }
    }
}

