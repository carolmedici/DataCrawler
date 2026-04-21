using DataCrawler.Worker.Data;
using DataCrawler.Worker.Models;
using HtmlAgilityPack;

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
        _logger.LogInformation("Initiating capture cycle: {time}", DateTimeOffset.Now);

        using (var scope = _serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.EnsureCreatedAsync(stoppingToken);

            using var http = new HttpClient();      
            http.DefaultRequestHeaders.Add("User-Agent", "DataCrawlerBot/1.0");
          
            try 
            {
                var responseBtc = await http.GetStringAsync("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd", stoppingToken);
                using var doc = System.Text.Json.JsonDocument.Parse(responseBtc);
                var precoBtc = doc.RootElement.GetProperty("bitcoin").GetProperty("usd").GetDecimal();

                db.Captures.Add(new CapturedData { 
                    Source = "CoinGecko API", 
                    Content = $"BTC/USD: ${precoBtc}" 
                });
                _logger.LogInformation("Bitcoin captured: ${value}", precoBtc);
            }
            catch (Exception ex) { _logger.LogError("Error CoinGecko: {error}", ex.Message); }
         
            try 
            {
                var responseUsd = await http.GetStringAsync("https://economia.awesomeapi.com.br/last/USD-BRL", stoppingToken);
                using var doc = System.Text.Json.JsonDocument.Parse(responseUsd);
                var cotacaoUsd = doc.RootElement.GetProperty("USDBRL").GetProperty("bid").GetString();

                db.Captures.Add(new CapturedData { 
                    Source = "AwesomeAPI", 
                    Content = $"Dollar: R$ {cotacaoUsd}" 
                });
                _logger.LogInformation("Dollar captured: R$ {value}", cotacaoUsd);
            }
            catch (Exception ex) { _logger.LogError("Error AwesomeAPI: {error}", ex.Message); }    

            await db.SaveChangesAsync(stoppingToken);
        }  
        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
    }
}
}