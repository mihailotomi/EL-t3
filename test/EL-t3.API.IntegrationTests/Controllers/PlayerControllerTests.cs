using System.Net;
using System.Text.Json;
using EL_t3.API.Tests.Common;
using EL_t3.Application.Player.DTOs;
using EL_t3.Domain.Entities;
using EL_t3.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EL_t3.API.Tests.Controllers;

[Collection("ApiIntegrationTests")]
public class PlayerControllerTests : BaseControllerTests
{
    private readonly JsonSerializerOptions serializer = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PlayerControllerTests(ApiFactory factory) : base(factory)
    {
        List<Player> players = [
            Player.Create("Stephen", "Curry", new DateOnly(1988, 3, 14), "USA", "https://example.com/curry.jpg"),
            Player.Create("LeBron", "James", new DateOnly(1984, 12, 30), "USA", "https://example.com/james.jpg"),
            Player.Create("Kevin", "Durant", new DateOnly(1988, 9, 29), "USA", "https://example.com/durant.jpg"),
            Player.Create("Giannis", "Antetokounmpo", new DateOnly(1994, 12, 6), "GRC", "https://example.com/giannis.jpg"),
            Player.Create("Luka", "Doncic", new DateOnly(1999, 2, 28), "SVN", "https://example.com/doncic.jpg"),
            Player.Create("Nikola", "Jokic", new DateOnly(1995, 2, 19), "SRB", "https://example.com/jokic.jpg"),
            Player.Create("Joel", "Embiid", new DateOnly(1994, 3, 16), "CMR", "https://example.com/embiid.jpg"),
            Player.Create("Ja", "Morant", new DateOnly(1999, 8, 10), "USA", "https://example.com/morant.jpg"),
            Player.Create("Jayson", "Tatum", new DateOnly(1998, 3, 3), "USA", "https://example.com/tatum.jpg"),
            Player.Create("Devin", "Booker", new DateOnly(1996, 10, 30), "USA", "https://example.com/booker.jpg"),
            Player.Create("Anthony", "Davis", new DateOnly(1993, 3, 11), "USA", "https://example.com/davis.jpg"),
            Player.Create("Damian", "Lillard", new DateOnly(1990, 7, 15), "USA", "https://example.com/lillard.jpg"),
            Player.Create("Trae", "Young", new DateOnly(1998, 9, 19), "USA", "https://example.com/young.jpg"),
            Player.Create("Karl-Anthony", "Towns", new DateOnly(1995, 11, 15), "DOM", "https://example.com/towns.jpg"),
            Player.Create("Donovan", "Mitchell", new DateOnly(1996, 9, 7), "USA", "https://example.com/mitchell.jpg")
        ];

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDatabaseContext>();
        dbContext.Players.AddRange(players);
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
}