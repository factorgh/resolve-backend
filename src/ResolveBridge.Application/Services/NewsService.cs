using Microsoft.EntityFrameworkCore;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;
using ResolveBridge.Domain.Entities;

namespace ResolveBridge.Application.Services;

public class NewsService : INewsService
{
    private readonly IApplicationDbContext _context;

    public NewsService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<NewsArticleDto>> GetPublishedArticlesAsync()
    {
        return await _context.NewsArticles
            .Where(a => a.IsPublished)
            .OrderByDescending(a => a.PublishedAt)
            .Select(a => new NewsArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Summary = a.Summary,
                Tag = a.Tag,
                Icon = a.Icon,
                ImageUrl = a.ImageUrl,
                ExternalUrl = a.ExternalUrl,
                ReadingTimeMinutes = a.ReadingTimeMinutes,
                PublishedAt = a.PublishedAt
            })
            .ToListAsync();
    }

    public async Task<NewsArticleDto> GetArticleByIdAsync(Guid id)
    {
        var a = await _context.NewsArticles.FirstOrDefaultAsync(x => x.Id == id);
        if (a == null) throw new KeyNotFoundException("Article not found");

        return new NewsArticleDto
        {
            Id = a.Id,
            Title = a.Title,
            Content = a.Content,
            Summary = a.Summary,
            Tag = a.Tag,
            Icon = a.Icon,
            ImageUrl = a.ImageUrl,
            ExternalUrl = a.ExternalUrl,
            ReadingTimeMinutes = a.ReadingTimeMinutes,
            PublishedAt = a.PublishedAt
        };
    }

    public async Task<NewsArticleDto> CreateArticleAsync(CreateNewsArticleRequestDto request)
    {
        var article = new NewsArticle
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            Summary = request.Summary,
            Tag = request.Tag,
            Icon = request.Icon,
            ExternalUrl = request.ExternalUrl,
            ReadingTimeMinutes = request.ReadingTimeMinutes,
            IsPublished = request.IsPublished,
            PublishedAt = request.IsPublished ? DateTime.UtcNow : default,
            CreatedAt = DateTime.UtcNow
        };

        _context.NewsArticles.Add(article);
        await _context.SaveChangesAsync(default);

        return new NewsArticleDto
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Summary = article.Summary,
            Tag = article.Tag,
            Icon = article.Icon,
            ExternalUrl = article.ExternalUrl,
            ReadingTimeMinutes = article.ReadingTimeMinutes,
            PublishedAt = article.PublishedAt
        };
    }

    public async Task<bool> DeleteArticleAsync(Guid id)
    {
        var article = await _context.NewsArticles.FindAsync(id);
        if (article == null) return false;

        _context.NewsArticles.Remove(article);
        await _context.SaveChangesAsync(default);
        return true;
    }
}
