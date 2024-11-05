using EL_t3.Application.Common.Interfaces.Gateway;
using EL_t3.Domain.Entities;
using EL_t3.Infrastructure.Gateway.Contracts;
using EL_t3.Infrastructure.Gateway.Extensions;
using EL_t3.Infrastructure.Gateway.Validators;
using System.Net.Http.Json;

namespace EL_t3.Infrastructure.Gateway;

public class EuroleagueApiGateway : IClubGateway, IPlayerBySeasonGateway
{
    private readonly HttpClient _client;

    public EuroleagueApiGateway(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("euroleague-api");
    }

    public async Task<(IEnumerable<Club> clubs, IEnumerable<string> errors)> FetchClubsBySeasonAsync(int season)
    {
        var response = await _client.GetFromJsonAsync<GatewayListResponse<GatewayClub>>($"/v2/competitions/E/seasons/E{season}/clubs");

        if (response == null || response.Data == null)
        {
            throw new Exception("Failed to fetch clubs from the API.");
        }

        var validator = new GatewayClubValidator();

        var validClubs = new List<Club>();
        var validationErrors = new List<string>();

        foreach (var gc in response.Data)
        {
            var validationResult = validator.Validate(gc);
            if (validationResult.IsValid)
            {
                validClubs.Add(gc.MapToClubEntity());
            }
            else
            {
                validationErrors.AddRange(validationResult.Errors.Select(e => $"{gc.Code} {e.ErrorMessage}"));
            }
        }

        return (validClubs, validationErrors);
    }

    public async Task<(IEnumerable<PlayerSeason> playerSeasons, IEnumerable<string> errors)> FetchPlayerSeasonsBySeasonAsync(int season)
    {
        var response = await _client.GetFromJsonAsync<GatewayListResponse<GatewayPlayerSeason>>($"/v2/competitions/E/seasons/E{season}/people?personType=J");

        var validator = new GatewayPlayerSeasonValidator();

        var validPlayerSeasons = new List<PlayerSeason>();
        var validationErrors = new List<string>();

        if (response == null || response.Data == null)
        {
            throw new Exception("Failed to fetch players from the API.");
        }

        foreach (var ps in response.Data)
        {
            var validationResult = validator.Validate(ps);
            if (validationResult.IsValid)
            {
                validPlayerSeasons.Add(ps.MapToPlayerSeasonEntity());
            }
            else
            {
                validationErrors.AddRange(validationResult.Errors.Select(e => $"{ps.Person.Name} {e.ErrorMessage}"));
            }
        }

        return (validPlayerSeasons, validationErrors);
    }
}