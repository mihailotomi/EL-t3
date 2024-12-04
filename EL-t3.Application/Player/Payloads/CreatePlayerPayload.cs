namespace EL_t3.Application.Player.Payloads;

public record CreatePlayerPayload(
    string FirstName,
    string LastName,
    string? ImageUrl,
    DateOnly BirthDate,
    string? Country
);

