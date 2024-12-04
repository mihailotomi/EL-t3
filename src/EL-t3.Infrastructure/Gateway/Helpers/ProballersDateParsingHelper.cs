using System.Globalization;

namespace EL_t3.Infrastructure.Gateway.Helpers;

public class ProballersDateParsingHelper
{
    private static readonly Dictionary<string, int> Months = new()
     {
        { "Jan", 1 },
        { "Feb", 2 },
        { "Mar", 3 },
        { "Apr", 4 },
        { "May", 5 },
        { "Jun", 6 },
        { "Jul", 7 },
        { "Aug", 8 },
        { "Sep", 9 },
        { "Oct", 10 },
        { "Nov", 11 },
        { "Dec", 12 }
    };

    public static string ParseBirthDate(string birthDateAndAgeString)
    {
        var parts = birthDateAndAgeString.Split(' ');

        int day = int.Parse(parts[1].Replace(",", ""), CultureInfo.InvariantCulture);
        int month = Months[parts[0]];
        int year = int.Parse(parts[2], CultureInfo.InvariantCulture);

        var date = new DateTime(year, month, day);
        var formattedDate = $"{date:yyyy-MM-dd}";

        return formattedDate;
    }

    private static string PadZero(int number)
    {
        return number.ToString().PadLeft(2, '0');
    }
}