
using dotnet_sample.Data;
using dotnet_sample.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContext>(connString); //dependency Injection

var app = builder.Build();

app.MapGamesEndpoints();

app.MapGenresEndpoints();

await app.MigrateDbAsync();

app.Run();