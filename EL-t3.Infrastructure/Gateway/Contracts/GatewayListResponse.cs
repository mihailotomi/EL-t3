namespace EL_t3.Infrastructure.Gateway.Contracts;

public record GatewayListResponse<T>(int Total, IEnumerable<T> Data);