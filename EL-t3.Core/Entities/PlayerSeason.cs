namespace EL_t3.Core.Entities;
public class PlayerSeason
{
    public int Id { get; set; }
    public int ClubId { get; set; }
    public Club? Club { get; set; }
    public int PlayerId { get; set; }
    public Player? Player { get; set; }
    public int Season { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

