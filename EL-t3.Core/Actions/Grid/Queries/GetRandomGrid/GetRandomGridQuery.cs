using MediatR;

namespace EL_t3.Core.Actions.Grid.Queries.GetRandomGrid;

public record GetRandomGridQuery() : IRequest<Entities.Grid>;