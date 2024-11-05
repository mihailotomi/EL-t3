using System.ComponentModel;
using System.Text.Json.Serialization;

namespace EL_t3.Domain.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GridItemType
{
    [Description("CLUB")]
    CLUB,

    [Description("COUNTRY")]
    COUNTRY,
}

public class GridItem
{
    public GridItemType Type { get; set; }

    public required object Item { get; set; }
}