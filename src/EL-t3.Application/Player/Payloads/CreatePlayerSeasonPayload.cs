namespace EL_t3.Application.Player.Payloads;

public record CreatePlayerSeasonPayload(
    string FirstName,
    string LastName,
    string? ImageUrl,
    DateOnly BirthDate,
    string? Country,
    int Season,
    string ClubCode,
    DateOnly StartedAt,
    DateOnly EndedAt
);