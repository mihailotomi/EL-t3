namespace EL_t3.Infrastructure.Gateway.Helpers;

public class ProballersEuroleagueUriHelper : IProballersUriHelper
{
    // A club can have multiple pages on proballers - because Crvena Zvezda is FMP
    // Thank you doc. Nebojsa Covic
    private static readonly Dictionary<string, string[]> ClubUriMap = new()
    {
        {"PAR", ["565/partizan-belgrade"]},
        {"ULK", ["552/fenerbahce-istanbul"]},
        {"MAD", ["160/real-madrid"]},
        {"BER", ["384/alba-berlin"]},
        {"IST", ["543/anadolu-efes-istanbul"]},
        {"MCO", ["283/as-monaco"]},
        {"BAS", ["155/baskonia-vitoria-gasteiz"]},
        {"RED", ["560/crvena-zvezda", "561/crvena-zvezda-belgrade"]},
        {"MIL", ["178/ea7-emporio-armani-milan"]},
        {"BAR", ["148/fc-barcelona"]},
        {"MUN", ["2020/fc-bayern-munich"]},
        {"ASV", ["1/ldlc-asvel"]},
        {"TEL", ["609/maccabi-playtika-tel-aviv"]},
        {"OLY", ["188/olympiacos-piraeus"]},
        {"PAN", ["185/panathinaikos-athens"]},
        {"PAM", ["153/valencia-basket"]},
        {"VIR", ["165/virtus-segafredo-bologna"]},
        {"ZAL", ["649/zalgiris-kaunas"]},
        {"CSK", ["581/cska-moscow"]},
        {"UNK", ["382/unics-kazan"]},
        {"DYR", ["1733/zenit-st-petersburg"]},
        {"KHI", ["579/khimki-moscow-region"]},
        {"BUD", ["559/buducnost-voli"]},
        {"DAR", ["547/darussafaka"]},
        {"CAN", ["157/dreamland-gran-canaria"]},
        {"BAM", ["409/bamberg-baskets"]},
        {"MAL", ["161/unicaja-malaga"]},
        {"GAL", ["553/galatasaray"]},
        {"CED", ["1359/cedevita-olimpija"]},
    };

    /// <summary>
    /// Map club code to a URI on proballers website.
    /// </summary>
    /// <param name="clubCode">Club code in Euroleague</param>
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