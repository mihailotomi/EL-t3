using EL_t3.Domain.Entities;

namespace EL_t3.Application.Grid.DTOs;

public static class DtoMapping
{
    public static GridItemDTO ToItemDTO(this ClubGridItemDTO c) =>
        new GridItemDTO(Type: GridItemType.CLUB, Item: c.Id.ToString(), ImageUrl: c.ImageUrl);

    public static GridItemDTO ToItemDTO(this CountryGridItemDTO c) =>
        new GridItemDTO(Type: GridItemType.COUNTRY, Item: c.Code, ImageUrl: c.ImageUrl);
}

