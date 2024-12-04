using System.Linq.Expressions;

namespace EL_t3.Domain.Entities;

public class Player : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateOnly BirthDate { get; private set; }
    public string? Country { get; private set; }
    private string? _imageUrl;
    public string? ImageUrl
    {
        get => _imageUrl;
        private set
        {
            ValidateImageUrl(value);
            _imageUrl = value;
        }
    }
    public ICollection<PlayerSeason> SeasonsPlayed { get; private set; } = [];

    private Player() { }

    private Player(
        string firstName,
        string lastName,
        DateOnly birthDate,
        string? country,
        string? imageUrl
    )
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Country = country;
        ImageUrl = imageUrl;
    }

    private static void ValidateImageUrl(string? imageUrl)
    {
        if (imageUrl is not null)
        {
            var imageUriValid = Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute);
            if (!imageUriValid)
            {
                throw new ArgumentException("ImageUrl must be a valid web url or null!");
            }
        }
    }

    public void UpdateImageUrlIfNone(string imageUrl)
    {
        if (ImageUrl is null)
        {
            ImageUrl = imageUrl;
        }
    }

    public static Player Create(
        string firstName,
        string lastName,
        DateOnly birthDate,
        string? country,
        string? imageUrl
    )
    {
        return new Player(firstName, lastName, birthDate, country, imageUrl);
    }

    public static Expression<Func<Player, Player, Player>> Upserter = (Player pDb, Player pIns) => new Player()
    {
        FirstName = pDb.FirstName,
        LastName = pDb.LastName,
        BirthDate = pDb.BirthDate,
        ImageUrl = pDb.ImageUrl ?? pIns.ImageUrl,
        Country = pDb.Country ?? pIns.Country
    };
}

