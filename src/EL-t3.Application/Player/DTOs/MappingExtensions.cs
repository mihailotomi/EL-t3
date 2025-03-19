
namespace EL_t3.Application.Player.DTOs;   

public static class MappingExtensions{
    public static PlayerDTO ToPlayerDTO(this Domain.Entities.Player player)
    {
        return new PlayerDTO(
            Id: player.Id,
            FirstName: player.FirstName,
            LastName: player.LastName,
            BirthDate: player.BirthDate,
            Country: player.Country,
            ImageUrl: player.ImageUrl,
            $"{player.FirstName} {player.LastName}"
        );
    }
}