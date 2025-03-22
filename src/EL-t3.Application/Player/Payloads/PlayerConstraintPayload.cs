using System.ComponentModel;
using System.Text.Json.Serialization;
using EL_t3.Domain.Entities;

namespace EL_t3.Application.Player.Payloads;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GridItemType
{
    [Description("CLUB")]
    CLUB,

    [Description("COUNTRY")]
    COUNTRY,

    [Description("TEAMMATE")]
    TEAMMATE
}

public record PlayerConstraintPayload(GridItemType Type, string Item);

