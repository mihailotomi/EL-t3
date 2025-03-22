using EL_t3.Application.Player.Payloads;
using EL_t3.Domain.Entities;

namespace EL_t3.Application.Grid.DTOs;

public class ClubGridItemDTO
{
    public long Id { get; set; }
    public required string ImageUrl { get; set; }
}

public class CountryGridItemDTO
{
    public required string Code { get; set; }
    public required string ImageUrl { get; set; }
}

public record GridItemDTO(GridItemType Type, string Item, string ImageUrl);

public record GridDTO(IEnumerable<GridItemDTO> X, IEnumerable<GridItemDTO> Y);

