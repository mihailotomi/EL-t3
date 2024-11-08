using MediatR;

namespace EL_t3.Application.Player.Queries.PlayerAutocomplete;

public record PlayerAutocompleteQuery(string Search) : IRequest<IEnumerable<Domain.Entities.Player>>;