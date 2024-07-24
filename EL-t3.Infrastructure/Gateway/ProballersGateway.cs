using EL_t3.Core.Entities;
using EL_t3.Core.Interfaces.Gateway;
using EL_t3.Infrastructure.Gateway.Contracts;
using EL_t3.Infrastructure.Gateway.Helpers;
using HtmlAgilityPack;

namespace EL_t3.Infrastructure.Gateway;

public class ProballersGateway : IPlayerByClubGateway
{
    private readonly HttpClient _client;

    public ProballersGateway(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("proballers");
    }

    public async Task<(IEnumerable<PlayerSeason> playerSeasons, IEnumerable<string> errors)> FetchPlayersSeasonsByClubAsync(string clubCode)
    {
        var (intermediateDtoList, intermediateFailureList) = await GetIntermediateDtoList(clubCode);
        var playerSeasons = new List<PlayerSeason>();
        var failureList = intermediateFailureList;

        var semaphore = new SemaphoreSlim(10);
        var tasks = intermediateDtoList.Select(async intermediateDto =>
        {
            await semaphore.WaitAsync();
            try
            {
                var response = await _client.GetAsync(intermediateDto.PlayerUri);

                response.EnsureSuccessStatusCode();

                var htmlContent = await response.Content.ReadAsStringAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);


                var playerSeasonList = ProballersHtmlParsingHelper.ParsePlayerData(doc, intermediateDto.Seasons, clubCode);
                playerSeasons.AddRange(playerSeasonList);

            }
            catch (Exception e)
            {
                failureList.Add($"Player page failure: ${intermediateDto.PlayerUri} - ${e.Message}");
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
        return (playerSeasons, failureList);
    }


    private async Task<(IEnumerable<ProballersIntermediateDto>, IList<string>)> GetIntermediateDtoList(string clubCode)
    {
        var intermediateDtoList = new List<ProballersIntermediateDto>();
        var failures = new List<string>();
        var clubUris = ProballersClubUriHelper.GetClubUri(clubCode);
        foreach (var clubUri in clubUris)
        {
            var response = await _client.GetAsync($"/basketball/team/{clubUri}/all-time-roster");
            response.EnsureSuccessStatusCode();
            var htmlContent = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            var (iterationIntermediateDtos, iterationFailures) = ProballersHtmlParsingHelper.ParseIntermediateDtos(doc, clubCode);

            if (clubUris.Length == 1)
            {
                return (iterationIntermediateDtos, iterationFailures);
            }

            intermediateDtoList.AddRange(iterationIntermediateDtos);
            failures.AddRange(iterationFailures);
        }

        return (intermediateDtoList, failures);
    }
}