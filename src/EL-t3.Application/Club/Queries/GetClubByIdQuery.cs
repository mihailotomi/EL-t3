using EL_t3.Application.Common.Exceptions;
using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Common.Interfaces.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Application.Club.Queries;

public class GetClubById
{
    public record Query(long Id) : IRequest<Domain.Entities.Club>;

    public record Handler : IRequestHandler<Query, Domain.Entities.Club>
    {
        private readonly IAppDatabaseContext _dbContext;

        public Handler(IAppDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Entities.Club> Handle(Query request, CancellationToken cancellationToken)
        {
            var club = await _dbContext.Clubs.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (club is null)
            {
                throw new EntityNotFoundException("Club", request.Id);
            }

            return club;
        }
    }
}