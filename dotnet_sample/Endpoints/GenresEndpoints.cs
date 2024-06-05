﻿using dotnet_sample.Data;
using dotnet_sample.Mapping;
using Microsoft.EntityFrameworkCore;

namespace dotnet_sample.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                            .Select(genre => genre.ToDto())
                            .AsNoTracking()
                            .ToListAsync()
        );

        return group;
    }
}
