using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Player.Payloads;
using EL_t3.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Application.Player.Queries;


public class CheckPlayerConstraints
{
    public record Query(int Id, IEnumerable<PlayerConstraintPayload> Constraints) : IRequest<bool>;


    public record QueryHandler : IRequestHandler<Query, bool>
    {
        private readonly IAppDatabaseContext _context;

        public QueryHandler(IAppDatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
        {
            foreach (var constraint in request.Constraints)
            {
                var constraintResult = await ValidateConstraint(request.Id, constraint);
                if (!constraintResult)
                {
                    return false;
                }
            }
            return true;
        }

        private async Task<bool> ValidateConstraint(int playerId, PlayerConstraintPayload constraint)
        {
            return constraint.Type switch
            {
                GridItemType.CLUB => await ValidateClubConstraint(playerId, long.Parse(constraint.Item)),
                GridItemType.COUNTRY => await ValidateCountryConstraint(playerId, constraint.Item),
                _ => true
            };
        }

        private async Task<bool> ValidateCountryConstraint(int playerId, string code)
        {
            var num = await _context.Players
                         .Where(p => p.Id == playerId && p.Country == code)
                         .CountAsync();

            return num > 0;
        }

        private async Task<bool> ValidateClubConstraint(int playerId, long clubId)
        {
            var num = await _context.PlayerSeasons
                         .Where(ps => ps.ClubId == clubId && ps.PlayerId == playerId)
                         .CountAsync();

            return num > 0;
        }
    }

    public class ConstraintValidator : AbstractValidator<PlayerConstraintPayload>
    {
        public ConstraintValidator()
        {
            RuleFor(x => x.Item).NotEmpty().WithMessage("Item must not be empty.");
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleForEach(x => x.Constraints).SetValidator(new ConstraintValidator());
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
