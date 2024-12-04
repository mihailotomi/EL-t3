using EL_t3.Application.Club.Payloads;
using EL_t3.Application.Common.Interfaces.Gateway;
using EL_t3.Application.Player.Payloads;
using EL_t3.Infrastructure.Gateway.Contracts;
using EL_t3.Infrastructure.Gateway.Helpers;
using HtmlAgilityPack;

namespace EL_t3.Infrastructure.Gateway;

public class ProballersGateway : IPlayerByClubGateway, IAllClubsGateway
{
    private readonly HttpClient _client;
    private readonly ProballersNbaUriHelper _nbaUriHelper = new();
    private readonly ProballersEuroleagueUriHelper _euroleagueUriHelper = new();

    public ProballersGateway(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("proballers");
    }

    public async Task<(IEnumerable<CreatePlayerSeasonPayload> playerSeasons, IEnumerable<string> errors)> FetchPlayersSeasonsByClubAsync(string gatewayClubCode, bool isNba = false, CancellationToken cancellationToken = default)
    {
        var (intermediateDtoList, intermediateFailureList) = await GetIntermediateDtoList(gatewayClubCode, isNba, cancellationToken);
        var playerSeasons = new List<CreatePlayerSeasonPayload>();
        var failureList = intermediateFailureList;

        var semaphore = new SemaphoreSlim(10);
        var tasks = intermediateDtoList.Select(async intermediateDto =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                var response = await _client.GetAsync(intermediateDto.PlayerUri, cancellationToken);

                response.EnsureSuccessStatusCode();

                var htmlContent = await response.Content.ReadAsStringAsync(cancellationToken);

                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                var clubCodeInDb = isNba ? gatewayClubCode + "-NBA" : gatewayClubCode;
                var playerSeasonList = ProballersHtmlParsingHelper.ParsePlayerData(doc, intermediateDto.Seasons, clubCodeInDb);
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

    public async Task<(IEnumerable<CreateClubPayload> payloads, IEnumerable<string> errors)> FetchAllClubs(bool isNba = true, CancellationToken cancellationToken = default)
    {
        var clubs = new List<CreateClubPayload>();
        IProballersUriHelper uriHelper = GetUriHelper(isNba);
        var failures = new List<string>();
        var clubCodes = uriHelper.GetCodes();

        foreach (var code in clubCodes)
        {
            var clubUris = uriHelper.GetClubUri(code);
            if (clubUris == null || clubUris.Length == 0)
            {
                failures.Add($"No uri for club {code}");
                continue;
            }
            var response = await _client.GetAsync($"/basketball/team/{clubUris![0]}", cancellationToken);

            try
            {
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync(cancellationToken);

                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                try
                {
                    var club = ProballersHtmlParsingHelper.ParseClubData(doc, code);
                    clubs.Add(club);
                }
                catch (Exception e)
                {
                    failures.Add($"{e.Message}");
                }
            }
            catch
            {
                failures.Add($"Club page not found for club {code}");

            }

        }

        return (clubs, failures);
    }

    private async Task<(IEnumerable<ProballersIntermediateDto>, IList<string>)> GetIntermediateDtoList(string clubCode, bool isNba = false, CancellationToken cancellationToken = default)
    {
        var intermediateDtoList = new List<ProballersIntermediateDto>();
        IProballersUriHelper uriHelper = GetUriHelper(isNba);
        var failures = new List<string>();
        var clubUris = uriHelper.GetClubUri(clubCode);
        foreach (var clubUri in clubUris)
        {
            var response = await _client.GetAsync($"/basketball/team/{clubUri}/all-time-roster", cancellationToken);
            response.EnsureSuccessStatusCode();
            var htmlContent = await response.Content.ReadAsStringAsync(cancellationToken);

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

    private IProballersUriHelper GetUriHelper(bool isNba) => isNba ? _nbaUriHelper : _euroleagueUriHelper;
}