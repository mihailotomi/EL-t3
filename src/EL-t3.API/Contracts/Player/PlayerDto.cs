namespace EL_t3.API.Contracts.Player;

public record PlayerDto
    (
        int Id,
        string FirstName,
        string LastName,
        DateOnly BirthDate,
        string? Country,
        string? ImageUrl,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string FullName
    );