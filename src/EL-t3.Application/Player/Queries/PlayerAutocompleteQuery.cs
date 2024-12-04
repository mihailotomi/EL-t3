using EL_t3.Application.Common.Interfaces.Context;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Application.Player.Queries;

public class PlayerAutocomplete
{
    public record Query(string Search) : IRequest<IEnumerable<Domain.Entities.Player>>;

    public record QueryHandler : IRequestHandler<Query, IEnumerable<Domain.Entities.Player>>
    {
        private readonly IAppDatabaseContext _context;

        public QueryHandler(IAppDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Entities.Player>> Handle(Query request, CancellationToken cancellationToken)
        {
            var searchPattern = $"%{request.Search.ToUpper()}%";

            return await _context.Players
                .Where(p => EF.Functions.Like(p.FirstName + " " + p.LastName, searchPattern) ||
                        EF.Functions.Like(p.LastName + " " + p.FirstName, searchPattern))
                .ToListAsync(cancellationToken);
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        private readonly int minSearchLength = 2;
        private readonly int maxSearchLength = 50;

        public QueryValidator()
        {
            RuleFor(x => x.Search)
                .NotEmpty().WithMessage("Search field must not be empty")
                .MinimumLength(minSearchLength)
                .WithMessage($"Search field must contain at least {minSearchLength} characters")
                .MaximumLength(maxSearchLength)
                .WithMessage($"Search field must not contain more than {maxSearchLength} characters");
        }
    }
}