using System.Linq.Expressions;

namespace EL_t3.Domain.Entities;
public class Club : BaseEntity
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public string CrestUrl { get; private set; }

    private Club()
    {

    }

    private Club(string name, string code, string crestUrl)
    {
        Name = name;
        Code = code;

        var crestUriValid = Uri.IsWellFormedUriString(crestUrl, UriKind.Absolute);
        if (!crestUriValid)
        {
            throw new ArgumentException("CrestUrl must be a valid web url!");
        }

        CrestUrl = crestUrl;
    }

    public static Club Create(string name, string code, string crestUrl)
    {
        return new Club(name, code, crestUrl);
    }

    public static Expression<Func<Club, Club, Club>> Upserter = (Club cDb, Club cIns) => new Club()
    {
        Name = cDb.Name,
        Code = cDb.Code,
        CrestUrl = cDb.CrestUrl ?? cIns.CrestUrl
    };

}

