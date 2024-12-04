using EL_t3.Application.Club.Payloads;

namespace EL_t3.Application.Common.Interfaces.Gateway;

public interface IAllClubsGateway
{
    Task<(IEnumerable<CreateClubPayload> payloads, IEnumerable<string> errors)> FetchAllClubs(bool isNba = false, CancellationToken cancellationToken = default);
}