namespace EL_t3.Infrastructure.Gateway.Contracts;

public record GatewayClubImages(string? Crest);

public record GatewayClub(string Code, string Name, GatewayClubImages? Images);