using DataCrawler.Worker;
using DataCrawler.Worker.Data;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();