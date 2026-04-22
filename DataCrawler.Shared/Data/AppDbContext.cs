using Microsoft.EntityFrameworkCore;
using DataCrawler.Shared.Models;

namespace DataCrawler.Shared.Data;

public class AppDbContext : DbContext{
  
    public DbSet<CapturedData> Captures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {        
        options.UseSqlite("Data Source=/app/data/datacrawler.db");
    }
}