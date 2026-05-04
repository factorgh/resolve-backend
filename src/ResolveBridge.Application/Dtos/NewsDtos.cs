namespace ResolveBridge.Application.Dtos;

public class NewsArticleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? ExternalUrl { get; set; }
    public int ReadingTimeMinutes { get; set; }
    public DateTime PublishedAt { get; set; }
}

public class CreateNewsArticleRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string? ExternalUrl { get; set; }
    public int ReadingTimeMinutes { get; set; }
    public bool IsPublished { get; set; }
}
