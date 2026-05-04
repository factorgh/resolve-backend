using Microsoft.EntityFrameworkCore;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;

namespace ResolveBridge.Application.Services;

public class ProductService : IProductService
{
    private readonly IApplicationDbContext _context;

    public ProductService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<FinancialProductDto>> GetFeaturedProductsAsync()
    {
        return await _context.FinancialProducts
            .Include(p => p.Institution)
            .Where(p => p.IsFeatured && p.IsActive)
            .Select(p => new FinancialProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                InterestRate = p.InterestRate,
                ProductType = p.ProductType.ToString(),
                InstitutionName = p.Institution.Name,
                TrustScore = 95,
                MatchPercentage = 85
            })
            .ToListAsync();
    }

    public async Task<FinancialProductDto?> GetProductByIdAsync(Guid id)
    {
        var p = await _context.FinancialProducts
            .Include(prod => prod.Institution)
            .FirstOrDefaultAsync(prod => prod.Id == id);

        if (p == null) return null;

        return new FinancialProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            InterestRate = p.InterestRate,
            ProductType = p.ProductType.ToString(),
            InstitutionName = p.Institution.Name,
            TrustScore = 95,
            MatchPercentage = 85
        };
    }

    public async Task<List<FinancialProductDto>> SearchProductsAsync(ProductFilterRequestDto request)
    {
        var query = _context.FinancialProducts.Include(p => p.Institution).AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchTerm))
            query = query.Where(p => p.Name.Contains(request.SearchTerm) || p.Institution.Name.Contains(request.SearchTerm));

        if (!string.IsNullOrEmpty(request.ProductType))
            query = query.Where(p => p.ProductType.ToString() == request.ProductType);

        return await query
            .Select(p => new FinancialProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                InterestRate = p.InterestRate,
                ProductType = p.ProductType.ToString(),
                InstitutionName = p.Institution.Name,
                TrustScore = 92,
                MatchPercentage = 80
            })
            .ToListAsync();
    }

    public async Task<List<FinancialProductDto>> GetRecommendationsAsync(string userId)
    {
        var userGuid = Guid.Parse(userId);
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == userGuid);
        
        int baseMatch = user?.KycStatus == ResolveBridge.Domain.Enums.KycStatus.Verified ? 98 : 75;

        return await _context.FinancialProducts
            .Include(p => p.Institution)
            .Where(p => p.IsActive)
            .Take(3)
            .Select(p => new FinancialProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                InterestRate = p.InterestRate,
                ProductType = p.ProductType.ToString(),
                InstitutionName = p.Institution.Name,
                TrustScore = 96,
                MatchPercentage = baseMatch - (p.DisplayOrder * 2)
            })
            .ToListAsync();
    }
}
