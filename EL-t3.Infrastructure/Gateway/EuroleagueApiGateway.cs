using EL_t3.Core.Entities;
using EL_t3.Core.Interfaces.Gateway;
using EL_t3.Infrastructure.Gateway.Contracts;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

using EL_t3.Infrastructure.Gateway.Validators;

namespace EL_t3.Infrastructure.Gateway;

public class EuroleagueApiGateway : IClubGateway
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EuroleagueApiGateway(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<(IEnumerable<Club> clubs, IEnumerable<GatewayClubFailure> errors)> FetchClubsBySeasonAsync(int season)
    {
        var client = _httpClientFactory.CreateClient("euroleague-api");
        var response = await client.GetFromJsonAsync<GatewayListResponse<GatewayClub>>($"/v2/competitions/E/seasons/E{season}/clubs");

        if (response == null || response.Data == null)
        {
            throw new Exception("Failed to fetch clubs from the API.");
        }

        var validator = new GatewayClubValidator();

        var validClubs = new List<Club>();
        var validationErrors = new List<GatewayClubFailure>();

        foreach (var gc in response.Data)
        {
            var validationResult = validator.Validate(gc);
            if (validationResult.IsValid)
            {
                validClubs.Add(new Club
                {
                    Name = gc.Name,
                    Code = gc.Code,
                    CrestUrl = gc?.Images?.Crest
                });
            }
            else
            {
                validationErrors.Add(new GatewayClubFailure(gc.Code, validationResult.Errors.Select(e => e.ErrorMessage)));
            }
        }

        return (validClubs, validationErrors);
    }

}