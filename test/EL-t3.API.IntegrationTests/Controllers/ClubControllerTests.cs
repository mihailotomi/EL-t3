using EL_t3.API.Tests.Common;
using EL_t3.Domain.Entities;
using FluentAssertions;
using System.Net;


namespace EL_t3.API.Tests.Controllers;

[Collection("ApiIntegrationTests")]
public class ClubControllerTests : BaseControllerTests
{
    public ClubControllerTests(ApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetClubById_WhenExists_ReturnsOk()
    {
        var club = await SeedClub();

        var response = await client.GetAsync($"/clubs/{club.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetClubById_WhenDoesntExist_ReturnsNotFound()
    {
        var response = await client.GetAsync($"/clubs/{1}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<Club> SeedClub()
    {
        var club = Club.Create("Partizan", "PAR", "http://partizan.crest.com", false);
        dbContext.Clubs.Add(club);
        await dbContext.SaveChangesAsync();
        return club;
    }
}
