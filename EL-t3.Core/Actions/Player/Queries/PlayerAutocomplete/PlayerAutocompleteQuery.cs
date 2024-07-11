using MediatR;

namespace EL_t3.Core.Actions.Player.Queries.PlayerAutocomplete;

public record PlayerAutocompleteQuery(string Search) : IRequest<IEnumerable<Entities.Player>>;