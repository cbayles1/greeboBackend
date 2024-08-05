namespace backend.Models;

public class ChocolateBar {
    public int Id {get; set;}
    public int? CacaoPercent { get; set; }
    public int? Recency { get; set; }
    public int? ReviewYear { get; set; }
    public float? Rating { get; set; }
    public string? Company { get; set; }
    public string? CompanyLocation { get; set; }
    public string? BeanType { get; set; }
    public string? BroadOrigin { get; set; }
    public string? SpecificOrigin { get; set; }
}