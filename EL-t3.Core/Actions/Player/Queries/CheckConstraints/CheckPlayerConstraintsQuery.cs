using MediatR;

namespace EL_t3.Core.Actions.Player.Queries.CheckConstraints;

public record PlayerConstraint();
public sealed record PlayerClubConstraint(int ClubId) : PlayerConstraint;
public sealed record PlayerCountryConstraint(string CountryCode) : PlayerConstraint;

public record CheckPlayerConstraintsQuery(int Id, IEnumerable<PlayerConstraint> Constraints) : IRequest<bool>;