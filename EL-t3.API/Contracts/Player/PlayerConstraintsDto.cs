using System.ComponentModel;
using System.Text.Json.Serialization;
using EL_t3.Core.Entities;

namespace EL_t3.API.Contracts.Player;

[JsonConverter(typeof(JsonStringEnumConverter))]

public record PlayerConstraintDto(int Id, string Code, GridItemType? Type);


