using EL_t3.Application.Club.Payloads;
using EL_t3.Application.Common.Interfaces.Gateway;
using EL_t3.Application.Player.Payloads;
using EL_t3.Infrastructure.Gateway.Contracts;
using EL_t3.Infrastructure.Gateway.Extensions;
using EL_t3.Infrastructure.Gateway.Validators;
using System.Net.Http.Json;

namespace EL_t3.Infrastructure.Gateway;

public class EuroleagueApiGateway : IClubBySeasonGateway, IPlayerBySeasonGateway
{
    private readonly HttpClient _client;

    public EuroleagueApiGateway(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("euroleague-api");
    }

    public async Task<(IEnumerable<CreateClubPayload> payloads, IEnumerable<string> errors)> FetchClubsBySeasonAsync(int season)
    {
        var response = await _client.GetFromJsonAsync<GatewayListResponse<GatewayClub>>($"/v2/competitions/E/seasons/E{season}/clubs");

        if (response == null || response.Data == null)
        {
            throw new Exception("Failed to fetch clubs from the API.");
        }

        var validator = new GatewayClubValidator();

        var validClubs = new List<CreateClubPayload>();
        var validationErrors = new List<string>();

        foreach (var gc in response.Data)
        {
            var validationResult = validator.Validate(gc);
            if (validationResult.IsValid)
            {
                try
                {
                    var payload = gc.ToPayload();
                    validClubs.Add(payload);
                }
                catch (Exception e)
                {
                    validationErrors.Add(e.Message);
                }
            }
            else
            {
                validationErrors.AddRange(validationResult.Errors.Select(e => $"{gc.Code} {e.ErrorMessage}"));
            }
        }

        return (validClubs, validationErrors);
    }

    public async Task<(IEnumerable<CreatePlayerSeasonPayload> playerSeasons, IEnumerable<string> errors)> FetchPlayerSeasonsBySeasonAsync(int season)
    {
        var response = await _client.GetFromJsonAsync<GatewayListResponse<GatewayPlayerSeason>>($"/v2/competitions/E/seasons/E{season}/people?personType=J");

        var validator = new GatewayPlayerSeasonValidator();

        var validPlayerSeasons = new List<CreatePlayerSeasonPayload>();
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
                validPlayerSeasons.Add(ps.ToPayload());
            }
            else
            {
                validationErrors.AddRange(validationResult.Errors.Select(e => $"{ps.Person.Name} {e.ErrorMessage}"));
            }
        }

        return (validPlayerSeasons, validationErrors);
    }
}