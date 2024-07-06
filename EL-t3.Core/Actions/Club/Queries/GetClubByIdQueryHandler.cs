using EL_t3.Core.Exceptions;
using EL_t3.Core.Interfaces.Repository;
using MediatR;

namespace EL_t3.Core.Actions.Club.Queries;

public record GetClubByIdQueryHandler : IRequestHandler<GetClubByIdQuery, Entities.Club>
{
    private readonly IClubRepository _clubRepository;

    public GetClubByIdQueryHandler(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }

    public async Task<Entities.Club> Handle(GetClubByIdQuery request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.FindByIdAsync(request.Id);
        if (club is null)
        {
            throw new EntityNotFoundException("Club", request.Id);
        }

        return club;
    }
}
