namespace EL_t3.Infrastructure.Gateway.Contracts;

public record GatewayCountry(string Code, string Name);

public record GatewayPerson(string Name, GatewayCountry Country, string BirthDate);

public record GatewayImages(string Headshot);

public record GatewaySeason(int Year);

public record GatewayPlayerSeason
(
    GatewayPerson Person,
    string StartDate, string EndDate,
    GatewayImages Images,
    GatewayClub Club,
    GatewaySeason Season
);