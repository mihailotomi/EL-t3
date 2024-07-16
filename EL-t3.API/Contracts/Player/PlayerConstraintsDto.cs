using System.ComponentModel;
using System.Text.Json.Serialization;

namespace EL_t3.API.Contracts.Player;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PlayerConstraintType
{
    [Description("CLUB")]
    CLUB,

    [Description("COUNTRY")]
    COUNTRY,
}

public record PlayerConstraintDto(int Id, string Code, PlayerConstraintType? Type);


