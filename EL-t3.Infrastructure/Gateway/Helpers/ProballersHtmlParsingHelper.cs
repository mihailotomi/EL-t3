using EL_t3.Domain.Entities;
using EL_t3.Infrastructure.Gateway.Contracts;
using HtmlAgilityPack;

namespace EL_t3.Infrastructure.Gateway.Helpers;

public class ProballersHtmlParsingHelper
{
    public static (IEnumerable<ProballersIntermediateDto>, IList<string>) ParseIntermediateDtos(HtmlDocument doc, string clubCode)
    {
        var contentDiv = doc.DocumentNode.SelectSingleNode("//div[contains(@id, 'player-all')]");
        if (contentDiv == null)
        {
            throw new ArgumentNullException(nameof(contentDiv), $"Club page parsing error: No content for club {clubCode}");
        }

        var tables = contentDiv.SelectNodes(".//table");
        if (tables == null)
        {
            throw new ArgumentNullException(nameof(tables), $"Club page parsing error: No player tables for club {clubCode}");
        }

        var intermediateDtoList = new List<ProballersIntermediateDto>();
        var playerFailureList = new List<string>();

        foreach (var table in tables)
        {
            var tableBody = table.SelectSingleNode(".//tbody");

            if (tableBody == null)
            {
                playerFailureList.Add("Club page parsing error: Empty table body");
                continue;
            }

            foreach (var row in tableBody.SelectNodes(".//tr"))
            {
                try
                {
                    var intermediateDto = ParsePlayerTableRow(row);
                    intermediateDtoList.Add(intermediateDto);
                }
                catch (Exception e)
                {
                    playerFailureList.Add($"Club page parsing error: {e.Message}");
                }
            }
        }

        return (intermediateDtoList, playerFailureList);
    }

    public static IEnumerable<PlayerSeason> ParsePlayerData(HtmlDocument doc, IEnumerable<int> seasons, string clubCode)
    {
        var playerNameContainer = doc.DocumentNode.SelectSingleNode("//h1[contains(@class, 'identity__name')]");
        if (playerNameContainer == null || !playerNameContainer.HasChildNodes)
        {
            throw new ArgumentException(nameof(playerNameContainer), "Player name container is missing or empty");
        }

        var names = new List<string>();

        foreach (var node in playerNameContainer.ChildNodes)
        {
            {
                var text = node.InnerText.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    names.Add(text);
                }
            }
        }


        var imageUrlNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'identity__picture--player')]//img");
        var imageUrl = imageUrlNode?.GetAttributeValue("src", string.Empty);

        var profileDiv = doc.DocumentNode.SelectSingleNode("//div[@class='identity__stats__profil']");
        if (profileDiv is null)
        {
            throw new ArgumentException(nameof(profileDiv), "Player birth date container not found");
        }
        var dateOfBirthDiv = profileDiv.SelectSingleNode(".//div[span[contains(text(), 'Date of birth')]]");
        if (dateOfBirthDiv is null)
        {
            throw new ArgumentException(nameof(dateOfBirthDiv), "Player birth date container not found");
        }
        var birthDateAndAgeNode = dateOfBirthDiv.SelectSingleNode(".//span[@class='info']");

        if (birthDateAndAgeNode == null)
        {
            throw new ArgumentException(nameof(birthDateAndAgeNode), "Player birth date container not found");
        }

        var birthDateRaw = birthDateAndAgeNode?.InnerText.Trim();
        if (string.IsNullOrEmpty(birthDateRaw))
        {
            throw new ArgumentException(nameof(birthDateRaw), "Birth date is missing or empty");
        }

        var birthDate = ProballersDateParsingHelper.ParseBirthDate(birthDateRaw);

        var nationalityNode = doc.DocumentNode.SelectSingleNode("//ul[contains(@class, 'identity__profil')]/*[2]");
        var nationality = nationalityNode != null ? nationalityNode?.InnerText.Trim() : null;
        var country = nationality != null ? NationalityMapHelper.NationalityToCountryISO(nationality) : null;

        return seasons.Select(s => new PlayerSeason()
        {
            Player = new Player()
            {
                FirstName = names[0].ToUpper().Trim(),
                LastName = names[1].ToUpper().Trim(),
                ImageUrl = imageUrl == null || imageUrl.Contains("head-par-defaut") ? null : imageUrl,
                BirthDate = DateOnly.Parse(birthDate),
                Country = country,
            },
            Season = s,
            Club = new Club()
            {
                Name = "",
                Code = clubCode
            }
        });
    }

    private static ProballersIntermediateDto ParsePlayerTableRow(HtmlNode? row)
    {
        if (row == null)
        {
            throw new ArgumentNullException(nameof(row), "Row is null");
        }

        var columns = row.SelectNodes(".//td");
        if (columns == null)
        {
            throw new ArgumentNullException(nameof(row), "No columns");
        }

        var playerAnchor = columns[0].SelectSingleNode(".//a[@class='list-player-entry']");
        if (playerAnchor == null)
        {
            throw new ArgumentNullException(nameof(row), "Missing player URI");
        }

        var playerUri = playerAnchor.GetAttributeValue("href", string.Empty);
        var seasonsNode = columns?[3];
        if (seasonsNode == null)
        {
            throw new ArgumentNullException(nameof(row), $"No seasons for player with URI {playerUri}");
        }

        var seasons = new List<int>();
        foreach (var seasonAnchor in seasonsNode.SelectNodes(".//a"))
        {
            var seasonText = seasonAnchor.InnerText.Trim();
            if (int.TryParse(seasonText.Split('-')[0], out var seasonYear))
            {
                seasons.Add(seasonYear);
            }
        }


        return new ProballersIntermediateDto(playerUri, seasons);
    }
}