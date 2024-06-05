using dotnet_sample.Dtos;
using dotnet_sample.Entities;

namespace dotnet_sample.Mapping;

public static class GenreMapping
{
    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto(genre.Id, genre.Name);
    }
}
