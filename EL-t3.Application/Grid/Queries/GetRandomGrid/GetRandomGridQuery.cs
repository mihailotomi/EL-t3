using MediatR;

namespace EL_t3.Application.Grid.Queries.GetRandomGrid;

public record GetRandomGridQuery() : IRequest<Domain.Entities.Grid>;