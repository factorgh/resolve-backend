using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Interfaces;
using ResolveBridge.Application.Services;
using ResolveBridge.Infrastructure.Data;
using ResolveBridge.Infrastructure.Services;

namespace ResolveBridge.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IDateTimeService, DateTimeService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ILoanService, LoanService>();
        services.AddScoped<IResponseFactory, ResponseFactory>();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ISmsService, SmsService>();
        services.AddScoped<IPushNotificationService, PushNotificationService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<INewsService, NewsService>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration["Jwt:SecretKey"] ?? "your-super-secret-key-with-at-least-32-characters-for-security!";
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"] ?? "ResolveBridge",
                ValidAudience = configuration["Jwt:Audience"] ?? "ResolveBridgeApp",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        return services;
    }
}

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<ResolveBridge.Domain.Entities.User, ResolveBridge.Application.Dtos.UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Market, opt => opt.MapFrom(src => src.Market.ToString()))
            .ForMember(dest => dest.KycStatus, opt => opt.MapFrom(src => src.KycStatus.ToString()));

        CreateMap<ResolveBridge.Domain.Entities.Institution, ResolveBridge.Application.Dtos.InstitutionDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<ResolveBridge.Domain.Entities.FinancialProduct, ResolveBridge.Application.Dtos.FinancialProductDto>()
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.ToString()))
            .ForMember(dest => dest.AvailableMarkets, opt => opt.MapFrom(src => src.AvailableMarkets.Select(m => m.ToString()).ToList()));

        CreateMap<ResolveBridge.Domain.Entities.Application, ResolveBridge.Application.Dtos.ApplicationDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<ResolveBridge.Domain.Entities.LoanLifecycle, ResolveBridge.Application.Dtos.LoanLifecycleDto>()
            .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()));

        CreateMap<ResolveBridge.Domain.Entities.Payment, ResolveBridge.Application.Dtos.PaymentDto>();

        CreateMap<ResolveBridge.Domain.Entities.Notification, ResolveBridge.Application.Dtos.NotificationDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Channel, opt => opt.MapFrom(src => src.Channel.ToString()));

        CreateMap<ResolveBridge.Domain.Entities.NewsArticle, ResolveBridge.Application.Dtos.NewsArticleDto>();
    }
}
