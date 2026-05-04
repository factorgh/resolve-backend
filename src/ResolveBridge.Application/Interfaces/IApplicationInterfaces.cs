using Microsoft.EntityFrameworkCore;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Domain.Entities;

namespace ResolveBridge.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> AppUsers { get; }
    DbSet<UserDocument> UserDocuments { get; }
    DbSet<Institution> Institutions { get; }
    DbSet<FinancialProduct> FinancialProducts { get; }
    DbSet<LoanProductDetails> LoanProductDetails { get; }
    DbSet<BNPLProductDetails> BNPLProductDetails { get; }
    DbSet<InsuranceProductDetails> InsuranceProductDetails { get; }
    DbSet<ResolveBridge.Domain.Entities.Application> Applications { get; }
    DbSet<ApplicationDocument> ApplicationDocuments { get; }
    DbSet<ApplicationStatusHistory> ApplicationStatusHistories { get; }
    DbSet<LoanLifecycle> LoanLifecycles { get; }
    DbSet<Payment> Payments { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<NewsArticle> NewsArticles { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    string? PhoneNumber { get; }
    bool IsAuthenticated { get; }
    string[] Roles { get; }
    bool HasRole(string role);
}

public interface IDateTimeService
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
}

public interface ISmsService
{
    Task SendSmsAsync(string phoneNumber, string message);
}

public interface IPushNotificationService
{
    Task SendPushNotificationAsync(string userId, string title, string body, Dictionary<string, string>? data = null);
}

public interface IJwtTokenService
{
    string GenerateAccessToken(string userId, string email, string[] roles);
    string GenerateRefreshToken();
    (bool isValid, string? userId) ValidateRefreshToken(string token);
    bool ValidateToken(string token);
}

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
    Task<bool> DeleteFileAsync(string fileUrl);
    Task<Stream> DownloadFileAsync(string fileUrl);
}

public interface IOtpService
{
    string GenerateOtp();
    Task StoreOtpAsync(string identifier, string otp, int expiryMinutes = 10);
    Task<bool> VerifyOtpAsync(string identifier, string otp);
}

public interface IDomainEventService
{
    Task Publish<TEvent>(TEvent @event) where TEvent : class;
}

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<VerifyOtpResponseDto> VerifyOtpAsync(VerifyOtpRequestDto request);
    Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request);
    Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request);
    Task<UserDto> GetCurrentUserAsync(string userId);
    Task<bool> ValidateTokenAsync(string token);
    Task<ApiResponse<bool>> VerifyKycAsync(string userId, KycSubmissionRequestDto request);
}

public interface IUserService
{
    Task<UserDto> UpdateProfileAsync(string userId, UpdateProfileRequestDto request);
    Task<ApiResponse<bool>> SubmitKycAsync(string userId, KycSubmissionRequestDto request);
    Task<List<UserDocumentDto>> GetUserDocumentsAsync(string userId);
    Task<DashboardMetricsDto> GetDashboardMetricsAsync(string userId);
}

public interface IProductService
{
    Task<List<FinancialProductDto>> GetFeaturedProductsAsync();
    Task<FinancialProductDto?> GetProductByIdAsync(Guid id);
    Task<List<FinancialProductDto>> SearchProductsAsync(ProductFilterRequestDto request);
    Task<List<FinancialProductDto>> GetRecommendationsAsync(string userId);
}

public interface IApplicationService
{
    Task<ApiResponse<ApplicationDto>> CreateApplicationAsync(string userId, CreateApplicationRequestDto request);
    Task<ApiResponse<bool>> SubmitApplicationAsync(Guid applicationId);
    Task<ApiResponse<bool>> ProcessDecisionAsync(Guid applicationId, ApplicationDecisionRequestDto request);
    Task<ApplicationDto> GetApplicationByIdAsync(Guid id);
    Task<PagedResult<ApplicationSummaryDto>> GetUserApplicationsAsync(string userId, PagedRequest request, string? type = null);
}

public interface IAnalyticsService
{
    Task<AnalyticsSummaryDto> GetSummaryAsync(AnalyticsFilterRequestDto filter);
    Task<List<InstitutionAnalyticsDto>> GetInstitutionAnalyticsAsync(AnalyticsFilterRequestDto filter);
    Task<List<MarketAnalyticsDto>> GetMarketAnalyticsAsync();
}

public interface INotificationService
{
    Task<PagedResult<NotificationDto>> GetNotificationsAsync(string userId, NotificationFilterRequestDto filter);
    Task<bool> MarkAsReadAsync(Guid notificationId);
    Task<bool> MarkAllAsReadAsync(string userId);
}

public interface ILoanService
{
    Task<PagedResult<LoanLifecycleSummaryDto>> GetLoansAsync(LoanLifecycleFilterRequestDto filter);
    Task<List<PaymentDto>> GetPaymentsAsync(Guid loanId);
    Task<PaymentDto> RecordPaymentAsync(RecordPaymentRequestDto request);
    Task<DashboardSummaryDto> GetDashboardSummaryAsync(string userId);
}

public interface INewsService
{
    Task<List<NewsArticleDto>> GetPublishedArticlesAsync();
    Task<NewsArticleDto> GetArticleByIdAsync(Guid id);
    Task<NewsArticleDto> CreateArticleAsync(CreateNewsArticleRequestDto request);
    Task<bool> DeleteArticleAsync(Guid id);
}
