namespace DataCrawler.Worker.Models;
public class CapturedData
{
    public int Id { get; set; }
    public string Source { get; set; } 
    public string Content { get; set; }    
    public DateTime CapturedAt { get; set; } = DateTime.Now;
}