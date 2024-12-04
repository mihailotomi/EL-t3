namespace EL_t3.Infrastructure.Gateway.Helpers;

public class NationalityMapHelper
{
    private static readonly Dictionary<string, string> CommonNationalityMap = new Dictionary<string, string>
        {
            { "AMERICAN", "USA" },
            { "SERBIAN", "SRB" },
            { "SPANISH", "ESP" },
            { "TURKISH", "TUR" },
            { "FRENCH", "FRA" },
            { "RUSSIAN", "RUS" },
            { "LITHUANIAN", "LTU" },
            { "CROATIAN", "CRO" },
            { "GREEK", "GRE" },
            { "ITALIAN", "ITA" },
            { "GERMAN", "GER" }
        };

    /// <summary>
    /// Map nationality string to country code.
    /// </summary>
    /// <param name="nationality">Player nationality string (e.g., Serbian, American...)</param>
    /// <returns>3-letter country code in ISO 3166 format or null if not found.</returns>
    public static string? NationalityToCountryISO(string nationality)
    {
        CommonNationalityMap.TryGetValue(nationality.ToUpper(), out var countryCode);
        return countryCode;
    }
}
