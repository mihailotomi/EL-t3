using EL_t3.API.Tests.Common;
using EL_t3.Application.Player.DTOs;
using EL_t3.Application.Player.Payloads;
using EL_t3.Domain.Entities;
using EL_t3.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace EL_t3.API.Tests.Controllers;

[Collection("ApiIntegrationTests")]
public class PlayerControllerTests : BaseControllerTests
{
    private readonly JsonSerializerOptions serializer = new()
    {
        PropertyNameCaseInsensitive = true
    };
    private readonly List<Club> clubs;
    private readonly List<Player> players;
    private readonly List<PlayerSeason> playerSeasons;

    public PlayerControllerTests(ApiFactory factory) : base(factory)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDatabaseContext>();

        clubs = [
            Club.Create("Anadolu Efes Istanbul", "IST", "https://example.com/efes.png", false),
            Club.Create("Real Madrid", "RMD", "https://example.com/real.png", false),
            Club.Create("FC Barcelona", "FCB", "https://example.com/barca.png", false),
            Club.Create("Olympiacos Piraeus", "OLY", "https://example.com/oly.png", false),
            Club.Create("Fenerbahce Istanbul", "FEN", "https://example.com/fener.png", false),
            Club.Create("CSKA Moscow", "CSK", "https://example.com/cska.png", false),
            Club.Create("Maccabi Tel Aviv", "MTA", "https://example.com/maccabi.png", false),
            Club.Create("Panathinaikos Athens", "PAO", "https://example.com/pao.png", false),
            Club.Create("AS Monaco", "ASM", "https://example.com/monaco.png", false),
            Club.Create("Virtus Bologna", "VIR", "https://example.com/virtus.png", false),
            Club.Create("Zalgiris Kaunas", "ZAL", "https://example.com/zalgiris.png", false),
            Club.Create("EA7 Emporio Armani Milan", "MIL", "https://example.com/milan.png", false),
            Club.Create("Baskonia Vitoria-Gasteiz", "BAS", "https://example.com/baskonia.png", false),
            Club.Create("ALBA Berlin", "BER", "https://example.com/alba.png", false),
            Club.Create("Valencia Basket", "VAL", "https://example.com/valencia.png", false),
            Club.Create("Partizan Belgrade", "PAR", "https://example.com/partizan.png", false),
            Club.Create("Bayern Munich", "BAY", "https://example.com/bayern.png", false),
            Club.Create("ASVEL Villeurbanne", "ASV", "https://example.com/asvel.png", false)
        ];
        dbContext.Clubs.AddRange(clubs);
        dbContext.SaveChanges();

        players = [
            Player.Create("DIMITRIS", "DIAMANTIDIS", new DateOnly(1980, 5, 6), "GRC", "https://example.com/diamantidis.jpg"),
            Player.Create("VASSILIS", "SPANOULIS", new DateOnly(1982, 8, 7), "GRC", "https://example.com/spanoulis.jpg"),
            Player.Create("JUAN CARLOS", "NAVARRO", new DateOnly(1980, 6, 13), "ESP", "https://example.com/navarro.jpg"),
            Player.Create("SERGIO", "LLULL", new DateOnly(1987, 11, 15), "ESP", "https://example.com/llull.jpg"),
            Player.Create("NANDO", "DE COLO", new DateOnly(1987, 6, 23), "FRA", "https://example.com/decolo.jpg"),
            Player.Create("MILOS", "TEODOSIC", new DateOnly(1987, 3, 19), "SRB", "https://example.com/teodosic.jpg"),
            Player.Create("SHANE", "LARKIN", new DateOnly(1992, 10, 2), "USA", "https://example.com/larkin.jpg"),
            Player.Create("NICK", "CALATHES", new DateOnly(1989, 2, 7), "GRC", "https://example.com/calathes.jpg"),
            Player.Create("MIKE", "JAMES", new DateOnly(1990, 8, 18), "USA", "https://example.com/james.jpg"),
            Player.Create("JAN", "VESELY", new DateOnly(1990, 4, 24), "CZE", "https://example.com/vesely.jpg"),
            Player.Create("NIKOLA", "MIROTIC", new DateOnly(1991, 2, 11), "ESP", "https://example.com/mirotic.jpg"),
            Player.Create("SERGIO", "RODRIGUEZ", new DateOnly(1986, 6, 12), "ESP", "https://example.com/rodriguez.jpg"),
            Player.Create("TORNIKE", "SHENGELIA", new DateOnly(1991, 10, 5), "GEO", "https://example.com/shengelia.jpg"),
            Player.Create("WALTER", "TAVARES", new DateOnly(1992, 3, 22), "CPV", "https://example.com/tavares.jpg"),
            Player.Create("VASILIJE", "MICIC", new DateOnly(1994, 1, 13), "SRB", "https://example.com/micic.jpg")
        ];

        dbContext.Players.AddRange(players);
        dbContext.SaveChanges();

        playerSeasons = [
            PlayerSeason.Create(clubs.First(c => c.Code == "IST").Id, players.First(p => p.FirstName == "SHANE").Id, 2023, new DateOnly(2023, 7, 1), new DateOnly(2024, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "RMD").Id, players.First(p => p.FirstName == "SERGIO" && p.LastName == "LLULL").Id, 2023, new DateOnly(2023, 7, 1), new DateOnly(2024, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "RMD").Id, players.First(p => p.FirstName == "WALTER").Id, 2023, new DateOnly(2023, 7, 1), new DateOnly(2024, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "MIL").Id, players.First(p => p.FirstName == "NIKOLA").Id, 2023, new DateOnly(2023, 7, 1), new DateOnly(2024, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "RMD").Id, players.First(p => p.FirstName == "NIKOLA").Id, 2014, new DateOnly(2014, 7, 1), new DateOnly(2015, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "FCB").Id, players.First(p => p.FirstName == "NIKOLA").Id, 2021, new DateOnly(2021, 7, 1), new DateOnly(2022, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "FEN").Id, players.First(p => p.FirstName == "JAN").Id, 2021, new DateOnly(2021, 7, 1), new DateOnly(2022, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "CSK").Id, players.First(p => p.FirstName == "MIKE").Id, 2019, new DateOnly(2019, 7, 1), new DateOnly(2020, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "PAO").Id, players.First(p => p.FirstName == "NICK").Id, 2014, new DateOnly(2014, 7, 1), new DateOnly(2015, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "FCB").Id, players.First(p => p.FirstName == "NICK").Id, 2021, new DateOnly(2021, 7, 1), new DateOnly(2022, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "VIR").Id, players.First(p => p.FirstName == "TORNIKE").Id, 2023, new DateOnly(2023, 7, 1), new DateOnly(2024, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "CSK").Id, players.First(p => p.FirstName == "NANDO").Id, 2018, new DateOnly(2018, 7, 1), new DateOnly(2019, 6, 30)),
            PlayerSeason.Create(clubs.First(c => c.Code == "FEN").Id, players.First(p => p.FirstName == "NANDO").Id, 2021, new DateOnly(2021, 7, 1), new DateOnly(2022, 6, 30)),
        ];

        dbContext.PlayerSeasons.AddRange(playerSeasons);
        dbContext.SaveChanges();
    }

    [Fact]
    public async Task NameAutocomplete_SearchTooShort_ReturnsBadRequest()
    {
        var response = await client.GetAsync($"/players/autocomplete?search=h");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task NameAutocomplete_HasMatches_ShouldOkWithList()
    {
        var response = await client.GetAsync($"/players/autocomplete?search=ic");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var playerList = JsonSerializer.Deserialize<IEnumerable<PlayerDTO>>(responseContent, serializer);

        playerList.Should().OnlyHaveUniqueItems().And.OnlyContain(p => p.FirstName.Contains("IC") || p.LastName.Contains("IC"));
    }

    [Fact]
    public async Task CheckConstraints_WhenMatchesTwoClubs_ShouldReturnOkWithTrue()
    {
        var solution = playerSeasons
            .GroupBy(ps => ps.PlayerId)
            .Where(g => g.Select(ps => ps.ClubId).Distinct().Count() == 2)
            .Select(g => new
            {
                PlayerId = g.Key,
                Clubs = g.Select(ps => ps.ClubId).Distinct().ToList()
            })
            .FirstOrDefault();

        var solutionPlayerId = solution!.PlayerId;
        var constrainedClubIds = solution.Clubs;

        IEnumerable<PlayerConstraintPayload> constraints = constrainedClubIds.Select(clubId => new PlayerConstraintPayload(GridItemType.CLUB, clubId.ToString()));

        var response = await client.PostAsJsonAsync($"/players/check-constraints/{solutionPlayerId}", constraints);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<bool>(responseContent, serializer);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task CheckConstraints_WhenMatchesAClubAndACountry_ShouldReturnOkWithTrue()
    {
        var solution = playerSeasons.First();

        var solutionPlayerId = solution!.PlayerId;
        var constrainedClubId = solution.ClubId;
        var constrainedCountry = players.First(p => p.Id == solutionPlayerId).Country;

        IEnumerable<PlayerConstraintPayload> constraints = [
            new(GridItemType.CLUB, constrainedClubId.ToString()),
            new(GridItemType.COUNTRY, constrainedCountry!)
        ];

        var response = await client.PostAsJsonAsync($"/players/check-constraints/{solutionPlayerId}", constraints);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<bool>(responseContent, serializer);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task CheckConstraints_WhenMatchesAClubAndTeamate_SouldReturnOkWithTrue(){
        var teammates = playerSeasons
            .GroupBy(ps => new { ps.ClubId, ps.Season })
            .Where(g => g.Count() > 1)
            .First();

        var solutionPlayerId = teammates.First().PlayerId;
        var teammateId = teammates.Skip(1).First().PlayerId;
        var clubId = teammates.Key.ClubId;

        IEnumerable<PlayerConstraintPayload> constraints = [
            new(GridItemType.CLUB, clubId.ToString()),
            new(GridItemType.TEAMMATE, teammateId.ToString())
        ];

        var response = await client.PostAsJsonAsync($"/players/check-constraints/{solutionPlayerId}", constraints);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<bool>(responseContent, serializer);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task CheckConstraints_WhenDoesNotMatchClubAndCountry_ShouldReturnOkWithFalse()
    {
        var solution = playerSeasons.First();

        var solutionPlayerId = solution!.PlayerId;
        var constrainedClubId = solution.ClubId;
        var constrainedCountry = players.First(p => p.Id == solutionPlayerId).Country;
        var wrongPlayerId = players.First(p => p.Country != constrainedCountry).Id;

        IEnumerable<PlayerConstraintPayload> constraints = [
            new(GridItemType.CLUB, constrainedClubId.ToString()),
            new(GridItemType.COUNTRY, constrainedCountry!)
        ];

        var response = await client.PostAsJsonAsync($"/players/check-constraints/{wrongPlayerId}", constraints);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<bool>(responseContent, serializer);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task CheckConstraints_WhenDoesNotMatchTwoClubs_ShouldReturnOkWithFalse()
    {
        var solution = playerSeasons
            .GroupBy(ps => ps.PlayerId)
            .Where(g => g.Select(ps => ps.ClubId).Distinct().Count() == 2)
            .Select(g => new
            {
                PlayerId = g.Key,
                Clubs = g.Select(ps => ps.ClubId).Distinct().ToList()
            })
            .FirstOrDefault();

        var constrainedClubIds = solution!.Clubs;

        var wrongSolutionPlayerId = playerSeasons
            .GroupBy(ps => ps.PlayerId)
            .Where(g => !g.Select(ps => ps.ClubId).Any(c => c == constrainedClubIds[0]))
            .Select(g => g.Key)
            .FirstOrDefault();

        IEnumerable<PlayerConstraintPayload> constraints = constrainedClubIds.Select(clubId => new PlayerConstraintPayload(GridItemType.CLUB, clubId.ToString()));

        var response = await client.PostAsJsonAsync($"/players/check-constraints/{wrongSolutionPlayerId}", constraints);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<bool>(responseContent, serializer);

        result.Should().BeFalse();
    }
}