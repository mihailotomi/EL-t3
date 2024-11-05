using EL_t3.Application.Common.Exceptions;
using EL_t3.Application.Common.Interfaces.Repository;
using MediatR;

namespace EL_t3.Application.Club.Queries;

public class GetClubById
{
    public record Query(int Id) : IRequest<Domain.Entities.Club>;

    public record Handler : IRequestHandler<Query, Domain.Entities.Club>
    {
        private readonly IClubRepository _clubRepository;

        public Handler(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<Domain.Entities.Club> Handle(Query request, CancellationToken cancellationToken)
        {
            var club = await _clubRepository.FindByIdAsync(request.Id);
            if (club is null)
            {
                throw new EntityNotFoundException("Club", request.Id);
            }

            return club;
        }
    }
}