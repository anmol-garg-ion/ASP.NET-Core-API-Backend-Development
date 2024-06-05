using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args); //"WebApplication" Provides apis to configure and host

var app = builder.Build(); //builds api


var todos = new List<Todo>();
// ------------------------------------------------------------------
app.MapGet("/todos", () => todos);


// ------------------------------------------------------------------
app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id) =>
{
    var targetTodo = todos.SingleOrDefault(t => id == t.Id);
    return targetTodo is null
           ? TypedResults.NotFound()
           : TypedResults.Ok(targetTodo);
});



// ------------------------------------------------------------------
app.MapPost("/todos", (Todo task) =>
{
    todos.Add(task);
    return TypedResults.Created("/todos/{id}", task);
});


// ------------------------------------------------------------------
app.MapDelete("/todos/{id}", (int id) =>
{
    todos.RemoveAll(t => id == t.Id);
    return TypedResults.NoContent();
});


// ------------------------------------------------------------------
app.Run();//runs the api

public record Todo(int Id, string Name, DateTime DueDate, bool IsCompleted);
