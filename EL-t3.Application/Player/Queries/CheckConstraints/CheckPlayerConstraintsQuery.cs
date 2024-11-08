using MediatR;

namespace EL_t3.Application.Player.Queries.CheckConstraints;

public record PlayerConstraint();
public sealed record PlayerClubConstraint(int Id) : PlayerConstraint;
public sealed record PlayerCountryConstraint(string CountryCode) : PlayerConstraint;

public record CheckPlayerConstraintsQuery(int Id, IEnumerable<PlayerConstraint> Constraints) : IRequest<bool>;