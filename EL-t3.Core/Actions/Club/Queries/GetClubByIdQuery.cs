using MediatR;

namespace EL_t3.Core.Actions.Club.Queries;

public record GetClubByIdQuery(int Id) : IRequest<Entities.Club>;