using EL_t3.Application.Common.Exceptions;
using EL_t3.Application.Common.Interfaces.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EL_t3.Application.Player.Commands;

public class MergePlayers
{
    public record Command(int Player1Id, int Player2Id, int dataFrom) : IRequest<Domain.Entities.Player>
    {
        public int DataFrom = dataFrom == 1 || dataFrom == 2 ? dataFrom : throw new ValidationException("DataFrom", "DataFrom should have a value of either 1 or 2!");
        public int PlayerToKeepId => DataFrom == 1 ? Player1Id : Player2Id;
        public int PlayerToDeleteId => DataFrom == 1 ? Player2Id : Player1Id;
    }

    public record CommandHandler : IRequestHandler<Command, Domain.Entities.Player>
    {
        private readonly IAppDatabaseContext _dbContext;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(IAppDatabaseContext dbContext, ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Domain.Entities.Player> Handle(Command request, CancellationToken cancellationToken)
        {
            var playerToRemove = await _dbContext.Players
                .Where(p => p.Id == request.PlayerToDeleteId)
                .Include(p => p.SeasonsPlayed)!
                .ThenInclude(sp => sp.Club)
                .FirstOrDefaultAsync(cancellationToken);

            var playerToKeep = await _dbContext.Players
                .Where(p => p.Id == request.PlayerToKeepId)
                .Include(p => p.SeasonsPlayed)!
                .ThenInclude(sp => sp.Club)
                .FirstOrDefaultAsync(cancellationToken);

            if (playerToRemove is null)
            {
                throw new EntityNotFoundException("Player", request.PlayerToDeleteId);
            }
            if (playerToKeep is null)
            {
                throw new EntityNotFoundException("Player", request.PlayerToKeepId);
            }

            // seasons that both instances have - can be deleted
            var duplicateSeasons = playerToRemove.SeasonsPlayed!
                .Where(season => playerToKeep.SeasonsPlayed!.Any(
                    ps => ps.Season == season.Season && ps.ClubId == season.ClubId)
                );

            _dbContext.PlayerSeasons.RemoveRange(duplicateSeasons);

            // seasons only the instance to be remove has
            var seasonsToTransfer = playerToRemove.SeasonsPlayed!
                .Except(duplicateSeasons)
                .ToList();

            foreach (var seasonPlayed in seasonsToTransfer)
            {
                playerToKeep.SeasonsPlayed!.Add(seasonPlayed);
            }
            if (playerToRemove.ImageUrl != null)
            {
                playerToKeep.UpdateImageUrlIfNone(playerToRemove.ImageUrl);
            }

            // player seasons that are duplicates (both player1 and player2 instances)
            _dbContext.Players.Remove(playerToRemove);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return playerToKeep;
        }
    }
}

