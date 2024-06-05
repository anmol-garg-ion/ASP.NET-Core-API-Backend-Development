
namespace dotnet_sample.Endpoints;

using dotnet_sample.Data;
using dotnet_sample.Dtos;
using dotnet_sample.Entities;
using dotnet_sample.Mapping;
using Microsoft.EntityFrameworkCore;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    // private static readonly List<GameSummaryDto> games = [
    // new (
    //     1,
    //     "Street Fighter II",
    //     "Fighting",
    //     19.99M,
    //     new DateOnly(1992, 7,15)),
    // new (
    //     2,
    //     "FIFA 24",
    //     "Sports",
    //     20.02M,
    //     new DateOnly(2010, 9, 30)),
    // new (
    //     3,
    //     "Modern Combat 5",
    //     "FPS",
    //     27.99M,
    //     new DateOnly(2011, 8, 14))
    // ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();
        //GET /games
        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Games
                     .Include(game => game.Genre)
                     .Select(game => game.ToGameSummaryDto())
                     .AsNoTracking()
                     .ToListAsync());


        // ------------------------------------------------------------------------------
        //Get /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);
            // GameDto? game = games.Find(game => game.Id == id);

            return game is null ?
                Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
           .WithName(GetGameEndpointName);


        // ------------------------------------------------------------------------------
        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            //Manually handling invalid inputs
            // if (string.IsNullOrEmpty(newGame.Name))
            // {
            //     return Results.BadRequest("Name is Required");
            // }

            //Manually Creating DTOs
            // GameDto game = new(
            //     games.Count + 1,
            //     newGame.Name,
            //     newGame.Genre,
            //     newGame.Price,
            //     newGame.ReleaseDate);
            // games.Add(game);

            //Creating Entities in POST Method Directly
            // Game game = new()
            // {
            //     Name = newGame.Name,
            //     Genre = dbContext.Genres.Find(newGame.GenreId),
            //     GenreId = newGame.GenreId,
            //     Price = newGame.Price,
            //     ReleaseDate = newGame.ReleaseDate
            // };

            Game game = newGame.ToEntity(); //Accessed from Mapping Entities to Dto

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            ;
            //Creating Dtos in POST Method directly
            // GameDto gameDto = new(
            //     game.Id,
            //     game.Name,
            //     game.Genre!.Name, //'!' here tells us that genre is never going to be null, since we are getting it using dbContext.Genres.Find(newGame.GenreId),
            //     game.Price,
            //     game.ReleaseDate
            // );
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
        });


        // ------------------------------------------------------------------------------
        //PUT /games/1
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            // var index = games.FindIndex(game => game.Id == id);
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            // games[index] = new GameSummaryDto(
            //     id,
            //     updatedGame.Name,
            //     updatedGame.Genre,
            //     updatedGame.Price,
            //     updatedGame.ReleaseDate
            //     );


            dbContext.Entry(existingGame)
                .CurrentValues
                .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });


        // ------------------------------------------------------------------------------
        //DELETE /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                     .Where(game => game.Id == id)
                     .ExecuteDeleteAsync();

            // games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });

        return group;
    }

}
