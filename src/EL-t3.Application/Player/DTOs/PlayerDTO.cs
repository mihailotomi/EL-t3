namespace EL_t3.Application.Player.DTOs;

public record PlayerDTO
    (
        long Id,
        string FirstName,
        string LastName,
        DateOnly BirthDate,
        string? Country,
        string? ImageUrl,
        string FullName
    );