using Microsoft.EntityFrameworkCore;
using DataCrawler.API.Data;
using DataCrawler.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/captures", async (AppDbContext db) =>
{
    return await db.Captures.ToListAsync();
})
.WithName("GetCaptures")
.WithOpenApi();

app.Run();