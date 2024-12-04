namespace EL_t3.Infrastructure.Gateway.Helpers;
public class ProballersNbaUriHelper : IProballersUriHelper
{
    // A club can have multiple pages on proballers - because Crvena Zvezda is FMP
    // Thank you doc. Nebojsa Covic
    private static readonly Dictionary<string, string[]> ClubUriMap = new()
    {
        {"PHI", ["119/philadelphia-76ers"]},
        {"MIL", ["114/milwaukee-bucks"]},
        {"CHI", ["103/chicago-bulls"]},
        {"CLE", ["104/cleveland-cavaliers"]},
        {"BOS", ["101/boston-celtics"]},
        {"LAC", ["111/los-angeles-clippers"]},
        {"MEM", ["127/memphis-grizzlies"]},
        {"ATL", ["100/atlanta-hawks"]},
        {"MIA", ["113/miami-heat"]},
        {"CHA", ["825/charlotte-hornets"]},
        {"UTA", ["126/utah-jazz"]},
        {"SAC", ["122/sacramento-kings"]},
        {"NYK", ["117/new-york-knicks"]},
        {"LAL", ["112/los-angeles-lakers"]},
        {"ORL", ["118/orlando-magic"]},
        {"DAL", ["105/dallas-mavericks"]},
        {"BKN", ["116/brooklyn-nets"]},
        {"DEN", ["106/denver-nuggets"]},
        {"IND", ["110/indiana-pacers"]},
        {"NOP", ["102/new-orleans-pelicans"]},
        {"DET", ["107/detroit-pistons"]},
        {"TOR", ["125/toronto-raptors"]},
        {"HOU", ["109/houston-rockets"]},
        {"SAS", ["123/san-antonio-spurs"]},
        {"PHX", ["120/phoenix-suns"]},
        {"OKC", ["1827/oklahoma-city-thunder", "/124/seattle-supersonics"]},
        {"MIN", ["115/minnesota-timberwolves"]},
        {"POR", ["121/portland-trail-blazers"]},
        {"GSW", ["108/golden-state-warriors"]},
        {"WAS", ["128/washington-wizards"]},
    };

    /// <summary>
    /// Map club code to a URI on proballers website.
    /// </summary>
    /// <param name="clubCode">Club code in the app</param>
    /// <returns>An array of club URIs (club can have multiple pages on Proballers - e. g. Crvena Zvezda)</returns>
    public string[] GetClubUri(string clubCode)
    {
        ClubUriMap.TryGetValue(clubCode, out var clubUris);
        if (clubUris == null)
        {
            throw new ArgumentException("No club with such code", clubCode);
        }

        return clubUris;
    }

    public IEnumerable<string> GetCodes()
    {
        return ClubUriMap.Keys;
    }
}

