using Microsoft.EntityFrameworkCore;
using NumberAPI.Data;
using NumberAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("NumbersInMemoryDB");
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("api/v1/numbers", async (AppDbContext context) =>
{
    var numItems = await context.NumberItems.ToListAsync();

    return Results.Ok(numItems);
});

app.MapPost("api/v1/numbers", async (AppDbContext context, NumberItem number) =>
{

    if (number == null)
    {
        throw new ArgumentNullException(nameof(number));
    }

    await context.NumberItems.AddAsync(number);
    await context.SaveChangesAsync();

    return Results.Created($"api/v1/{number.Id}", number);
});

app.Run();