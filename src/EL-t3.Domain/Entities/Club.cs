using System.Linq.Expressions;

namespace EL_t3.Domain.Entities;
public class Club : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public string CrestUrl { get; private set; } = string.Empty;
    public bool IsNba { get; private set; }

    private Club()
    {
    }

    private Club(string name, string code, string crestUrl, bool isNba = false)
    {
        Name = name;
        Code = code;
        IsNba = isNba;

        var crestUriValid = Uri.IsWellFormedUriString(crestUrl, UriKind.Absolute);
        if (!crestUriValid)
        {
            throw new ArgumentException("CrestUrl must be a valid web url!");
        }

        CrestUrl = crestUrl;
    }

    public static Club Create(string name, string code, string crestUrl, bool isNba = false)
    {
        return new Club(name, code, crestUrl, isNba);
    }

    public static Expression<Func<Club, Club, Club>> Upserter = (Club cDb, Club cIns) => new Club()
    {
        Name = cDb.Name,
        Code = cDb.Code,
        CrestUrl = cDb.CrestUrl ?? cIns.CrestUrl,
        IsNba = cDb.IsNba
    };
}

