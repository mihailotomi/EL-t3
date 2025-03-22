using System.ComponentModel;
using System.Text.Json.Serialization;

namespace EL_t3.Domain.Entities;

public abstract class GridItem: BaseEntity
{
}

public class ClubGridItem : GridItem
{
    public Club Club { get; set; } = null!;
    public long ClubId { get; set; }

    private ClubGridItem() { }

    public ClubGridItem(Club club)
    {
        Club = club;
        ClubId = club.Id;
    }
}

public class CountryGridItem : GridItem
{
    public string Country { get; set; } = null!;

    private CountryGridItem() { }

    public CountryGridItem(string country)
    {
        Country = country;
    }
}

public class TeammateGridItem : GridItem
{
    public Player Teammate { get; set; } = null!;
    public long TeammateId { get; set; }

    private TeammateGridItem() { }

    public TeammateGridItem(Player teammate)
    {
        Teammate = teammate;
        TeammateId = teammate.Id;
    }
}

