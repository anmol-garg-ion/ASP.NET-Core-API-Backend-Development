using System.ComponentModel.DataAnnotations;

namespace dotnet_sample.Dtos;

public record class CreateGameDto(
    [Required][StringLength(50)] string Name,
    // [Required][StringLength(20)] string Genre,
    int GenreId,
    [Range(1, 200)] decimal Price,
    DateOnly ReleaseDate
);