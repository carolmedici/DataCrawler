using Microsoft.EntityFrameworkCore;
using DataCrawler.API.Models; 

namespace DataCrawler.API.Data;

public class AppDbContext : DbContext{
    
    public DbSet<CapturedData> Captures { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {      
        options.UseSqlite("Data Source=../datacrawler.db");
    }
}