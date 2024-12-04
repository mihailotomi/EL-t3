namespace EL_t3.Application.Club.DTOs;

public class ClubDTO
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public string? CrestUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

