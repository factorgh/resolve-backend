using ResolveBridge.Domain.Enums;

namespace ResolveBridge.Domain.Entities;

public class NewsArticle
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty; // e.g. "Market Report", "Expert Tips"
    public string Icon { get; set; } = string.Empty; // e.g. "📈", "⚡"
    public string? ImageUrl { get; set; }
    public string? ExternalUrl { get; set; }
    public int ReadingTimeMinutes { get; set; }
    public bool IsPublished { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
