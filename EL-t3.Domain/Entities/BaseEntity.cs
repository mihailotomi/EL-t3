namespace EL_t3.Domain.Entities;

public class BaseEntity
{
    public long Id { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
}

