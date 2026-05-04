using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ResolveBridge.Application.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ResolveBridge.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public string? PhoneNumber => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.MobilePhone) ?? 
                                  _httpContextAccessor.HttpContext?.User?.FindFirstValue("phone_number");

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public string[] Roles => _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)
        .Select(c => c.Value).ToArray() ?? Array.Empty<string>();

    public bool HasRole(string role) => Roles.Contains(role);
}

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}

public class OtpService : IOtpService
{
    private static readonly Dictionary<string, (string otp, DateTime expiry)> _otpStore = new();

    public string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    public Task StoreOtpAsync(string identifier, string otp, int expiryMinutes = 10)
    {
        _otpStore[identifier] = (otp, DateTime.UtcNow.AddMinutes(expiryMinutes));
        return Task.CompletedTask;
    }

    public Task<bool> VerifyOtpAsync(string identifier, string otp)
    {
        if (!_otpStore.TryGetValue(identifier, out var stored))
            return Task.FromResult(false);

        if (DateTime.UtcNow > stored.expiry)
        {
            _otpStore.Remove(identifier);
            return Task.FromResult(false);
        }

        var isValid = stored.otp == otp;
        if (isValid)
            _otpStore.Remove(identifier);

        return Task.FromResult(isValid);
    }
}

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        _logger.LogInformation("Sending email to {To}: {Subject}", to, subject);
        return Task.CompletedTask;
    }
}

public class SmsService : ISmsService
{
    private readonly ILogger<SmsService> _logger;

    public SmsService(ILogger<SmsService> logger)
    {
        _logger = logger;
    }

    public Task SendSmsAsync(string phoneNumber, string message)
    {
        _logger.LogInformation("Sending SMS to {Phone}: {Message}", phoneNumber, message);
        return Task.CompletedTask;
    }
}

public class PushNotificationService : IPushNotificationService
{
    private readonly ILogger<PushNotificationService> _logger;

    public PushNotificationService(ILogger<PushNotificationService> logger)
    {
        _logger = logger;
    }

    public Task SendPushNotificationAsync(string userId, string title, string body, Dictionary<string, string>? data = null)
    {
        _logger.LogInformation("Sending push notification to {UserId}: {Title}", userId, title);
        return Task.CompletedTask;
    }
}

public class FileStorageService : IFileStorageService
{
    private readonly string _storagePath;
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(IConfiguration configuration, ILogger<FileStorageService> logger)
    {
        _storagePath = configuration["Storage:Path"] ?? "uploads";
        _logger = logger;
        Directory.CreateDirectory(_storagePath);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var fileId = Guid.NewGuid().ToString();
        var extension = Path.GetExtension(fileName);
        var storedFileName = $"{fileId}{extension}";
        var filePath = Path.Combine(_storagePath, storedFileName);

        using var stream = File.Create(filePath);
        await fileStream.CopyToAsync(stream);

        _logger.LogInformation("File uploaded: {FilePath}", filePath);
        return $"/uploads/{storedFileName}";
    }

    public Task<bool> DeleteFileAsync(string fileUrl)
    {
        var fileName = Path.GetFileName(fileUrl);
        var filePath = Path.Combine(_storagePath, fileName);
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return Task.FromResult(true);
        }
        
        return Task.FromResult(false);
    }

    public Task<Stream> DownloadFileAsync(string fileUrl)
    {
        var fileName = Path.GetFileName(fileUrl);
        var filePath = Path.Combine(_storagePath, fileName);
        
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found", fileName);
        
        var stream = File.OpenRead(filePath);
        return Task.FromResult<Stream>(stream);
    }
}
