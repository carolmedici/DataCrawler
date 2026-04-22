using Microsoft.EntityFrameworkCore;
using DataCrawler.Shared.Data;
using DataCrawler.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{   
    options.SwaggerEndpoint("v1/swagger.json", "v1"); 
    options.RoutePrefix = "swagger";
});


app.MapGet("/captures", async (AppDbContext db) =>
{
    return await db.Captures.ToListAsync();
})
.WithName("GetCaptures")
.WithOpenApi();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); 
}

app.Run();