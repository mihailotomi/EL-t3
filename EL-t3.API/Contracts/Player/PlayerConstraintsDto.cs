using EL_t3.Domain.Entities;

namespace EL_t3.API.Contracts.Player;

public record PlayerConstraintDto(int Id, string Code, GridItemType? Type);


