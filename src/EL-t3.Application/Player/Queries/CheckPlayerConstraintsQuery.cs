using EL_t3.Application.Common.Interfaces.Context;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Application.Player.Queries;

public record PlayerConstraint();
public sealed record PlayerClubConstraint(int Id) : PlayerConstraint;
public sealed record PlayerCountryConstraint(string CountryCode) : PlayerConstraint;

public class CheckPlayerConstraints
{
    public record Query(int Id, IEnumerable<PlayerConstraint> Constraints) : IRequest<bool>;


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

        private async Task<bool> ValidateConstraint(int playerId, PlayerConstraint constraint)
        {
            if (constraint is PlayerClubConstraint clubConstraint)
            {
                return await ValidateClubConstraint(playerId, clubConstraint);
            }
            if (constraint is PlayerCountryConstraint countryConstraint)
            {
                return await ValidateCountryConstraint(playerId, countryConstraint);
            }
            return false;
        }

        private async Task<bool> ValidateCountryConstraint(int playerId, PlayerCountryConstraint countryConstraint)
        {
            var num = await _context.Players
                         .Where(p => p.Id == playerId && p.Country == countryConstraint.CountryCode)
                         .CountAsync();

            return num > 0;
        }

        private async Task<bool> ValidateClubConstraint(int playerId, PlayerClubConstraint clubConstraint)
        {
            var num = await _context.PlayerSeasons
                         .Where(ps => ps.ClubId == clubConstraint.Id && ps.PlayerId == playerId)
                         .CountAsync();

            return num > 0;
        }
    }

    public class PlayerClubConstraintValidator : AbstractValidator<PlayerClubConstraint>
    {
        public PlayerClubConstraintValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id must not be empty and must be an integer.");
        }
    }

    public class PlayerCountryConstraintValidator : AbstractValidator<PlayerCountryConstraint>
    {
        public PlayerCountryConstraintValidator()
        {
            RuleFor(x => x.CountryCode).NotEmpty().WithMessage("Country Code must not be empty.");
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleForEach(x => x.Constraints).SetInheritanceValidator(v =>
            {
                v.Add(new PlayerClubConstraintValidator());
                v.Add(new PlayerCountryConstraintValidator());
            });
        }
    }
}
