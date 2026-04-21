using DataCrawler.Worker.Data;
using DataCrawler.Worker.Models;

namespace DataCrawler.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker is running in: {time}", DateTimeOffset.Now);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                
                await db.Database.EnsureCreatedAsync(stoppingToken);

                var newData = new CapturedData 
                { 
                    Source = "Coins", 
                    Content = "USD: 5.25" 
                };

                db.Captures.Add(newData);
                await db.SaveChangesAsync(stoppingToken);
                
                _logger.LogInformation("Data saved successfully!");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}