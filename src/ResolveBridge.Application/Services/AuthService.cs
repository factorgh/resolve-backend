using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;
using ResolveBridge.Domain.Entities;
using ResolveBridge.Domain.Enums;

using Microsoft.EntityFrameworkCore;

namespace ResolveBridge.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IFileStorageService _fileStorageService;

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration,
        ILogger<AuthService> logger,
        IEmailService emailService,
        ISmsService smsService,
        IApplicationDbContext context,
        IJwtTokenService jwtTokenService,
        IFileStorageService fileStorageService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
        _emailService = emailService;
        _smsService = smsService;
        _context = context;
        _jwtTokenService = jwtTokenService;
        _fileStorageService = fileStorageService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "User with this email already exists"
                };
            }

            existingUser = await _userManager.FindByNameAsync(request.PhoneNumber);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "User with this phone number already exists"
                };
            }

            // Create Identity user
            var identityUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false
            };

            var result = await _userManager.CreateAsync(identityUser, request.Password);
            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Failed to create user",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            // Assign Customer role
            await _userManager.AddToRoleAsync(identityUser, UserRole.Customer.ToString());

            // Create application user entity
            var appUser = new User
            {
                Id = Guid.Parse(identityUser.Id),
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                NationalId = request.IdNumber,
                IdType = request.IdType,
                Goals = request.Goals,
                MonthlyIncome = request.MonthlyIncome,
                LoanDurationPreference = request.LoanDuration,
                Role = UserRole.Customer,
                KycStatus = KycStatus.Pending,
                IsActive = true,
                EmailVerified = false,
                PhoneVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Title = request.Title,
                MaritalStatus = request.MaritalStatus,
                Gender = request.Gender,
                Nationality = request.Nationality,
                Dependants = request.Dependants,
                ResidentialAddress = request.ResidentialAddress,
                City = request.City,
                Mmda = request.Mmda,
                Landmark = request.Landmark,
                Employer = request.Employer,
                Sector = request.Sector,
                Occupation = request.Occupation,
                SsnitNo = request.SsnitNo,
                WorkAddress = request.WorkAddress,
                YearsWithEmployer = request.YearsWithEmployer
            };

            // Parse market and employment status
            if (Enum.TryParse<Market>(request.Market, true, out var market))
                appUser.Market = market;
            
            if (Enum.TryParse<EmploymentStatus>(request.EmploymentStatus, true, out var empStatus))
                appUser.EmploymentStatus = empStatus;

            _context.AppUsers.Add(appUser);
            await _context.SaveChangesAsync(default);

            // Generate tokens via IJwtTokenService
            var accessToken = _jwtTokenService.GenerateAccessToken(identityUser.Id, identityUser.Email, new[] { UserRole.Customer.ToString() });
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            return new AuthResponseDto
            {
                Success = true,
                Message = "Registration successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = new UserDto
                {
                    Id = appUser.Id,
                    Email = appUser.Email,
                    PhoneNumber = appUser.PhoneNumber,
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    Role = appUser.Role.ToString(),
                    Market = appUser.Market.ToString(),
                    KycStatus = appUser.KycStatus.ToString(),
                    EmailVerified = appUser.EmailVerified,
                    PhoneVerified = appUser.PhoneVerified,
                    CreatedAt = appUser.CreatedAt,
                    Title = appUser.Title,
                    MaritalStatus = appUser.MaritalStatus,
                    Gender = appUser.Gender,
                    Nationality = appUser.Nationality,
                    Dependants = appUser.Dependants,
                    ResidentialAddress = appUser.ResidentialAddress,
                    City = appUser.City,
                    Mmda = appUser.Mmda,
                    Landmark = appUser.Landmark,
                    Employer = appUser.Employer,
                    Sector = appUser.Sector,
                    Occupation = appUser.Occupation,
                    SsnitNo = appUser.SsnitNo,
                    WorkAddress = appUser.WorkAddress,
                    YearsWithEmployer = appUser.YearsWithEmployer
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return new AuthResponseDto
            {
                Success = false,
                Message = "An error occurred during registration"
            };
        }
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Identifier) ??
                      await _userManager.FindByNameAsync(request.Identifier);

            if (user == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid credentials"
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid credentials"
                };
            }

            // Generate tokens via IJwtTokenService
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email!, roles.ToArray());
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            // Fetch app user profile
            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(user.Id));

            if (appUser == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "User profile not found"
                };
            }

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = new UserDto
                {
                    Id = appUser.Id,
                    Email = appUser.Email,
                    PhoneNumber = appUser.PhoneNumber,
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    Role = appUser.Role.ToString(),
                    Market = appUser.Market.ToString(),
                    KycStatus = appUser.KycStatus.ToString(),
                    EmailVerified = appUser.EmailVerified,
                    PhoneVerified = appUser.PhoneVerified,
                    LastLoginAt = appUser.LastLoginAt,
                    Title = appUser.Title,
                    MaritalStatus = appUser.MaritalStatus,
                    Gender = appUser.Gender,
                    Nationality = appUser.Nationality,
                    Dependants = appUser.Dependants,
                    ResidentialAddress = appUser.ResidentialAddress,
                    City = appUser.City,
                    Mmda = appUser.Mmda,
                    Landmark = appUser.Landmark,
                    Employer = appUser.Employer,
                    Sector = appUser.Sector,
                    Occupation = appUser.Occupation,
                    SsnitNo = appUser.SsnitNo,
                    WorkAddress = appUser.WorkAddress,
                    YearsWithEmployer = appUser.YearsWithEmployer
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return new AuthResponseDto
            {
                Success = false,
                Message = "An error occurred during login"
            };
        }
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = false, // Don't validate lifetime for refresh token
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(request.RefreshToken, validationParameters, out SecurityToken validatedToken);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid refresh token"
                };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            // Generate new tokens
            var tokens = await GenerateTokensAsync(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Token refreshed successfully",
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                ExpiresAt = tokens.ExpiresAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid refresh token"
            };
        }
    }

    public async Task<VerifyOtpResponseDto> VerifyOtpAsync(VerifyOtpRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(request.PhoneNumber);
            if (user == null)
            {
                return new VerifyOtpResponseDto
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var storedOtp = await _userManager.GetAuthenticationTokenAsync(user, "PhoneVerification", "OTP");
            if (string.IsNullOrEmpty(storedOtp) || storedOtp != request.Otp)
            {
                return new VerifyOtpResponseDto
                {
                    Success = false,
                    Message = "Invalid or expired OTP"
                };
            }

            // Mark phone number as verified
            user.PhoneNumberConfirmed = true;
            await _userManager.UpdateAsync(user);

            // Remove OTP after successful verification
            await _userManager.RemoveAuthenticationTokenAsync(user, "PhoneVerification", "OTP");

            return new VerifyOtpResponseDto
            {
                Success = true,
                Message = "Phone number verified successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during OTP verification");
            return new VerifyOtpResponseDto
            {
                Success = false,
                Message = "An error occurred during verification"
            };
        }
    }

    public async Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that user doesn't exist
                return new ForgotPasswordResponseDto
                {
                    Success = true,
                    Message = "If an account with this email exists, a password reset link has been sent"
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{_configuration["App:BaseUrl"]}/reset-password?token={token}&email={request.Email}";

            await _emailService.SendEmailAsync(request.Email, "Reset Password", 
                $"Please click the following link to reset your password: {resetLink}");

            return new ForgotPasswordResponseDto
            {
                Success = true,
                Message = "Password reset link sent to your email"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password");
            return new ForgotPasswordResponseDto
            {
                Success = false,
                Message = "An error occurred while processing your request"
            };
        }
    }

    public async Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ResetPasswordResponseDto
                {
                    Success = false,
                    Message = "Invalid reset token"
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new ResetPasswordResponseDto
                {
                    Success = false,
                    Message = "Failed to reset password",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new ResetPasswordResponseDto
            {
                Success = true,
                Message = "Password reset successful"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            return new ResetPasswordResponseDto
            {
                Success = false,
                Message = "An error occurred while resetting your password"
            };
        }
    }

    public async Task<UserDto> GetCurrentUserAsync(string userId)
    {
        var appUser = await _context.AppUsers
            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));

        if (appUser == null)
            throw new KeyNotFoundException("User not found");

        return new UserDto
        {
            Id = appUser.Id,
            Email = appUser.Email,
            PhoneNumber = appUser.PhoneNumber,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Role = appUser.Role.ToString(),
            Market = appUser.Market.ToString(),
            KycStatus = appUser.KycStatus.ToString(),
            IsActive = appUser.IsActive,
            EmailVerified = appUser.EmailVerified,
            PhoneVerified = appUser.PhoneVerified,
            CreatedAt = appUser.CreatedAt,
            LastLoginAt = appUser.LastLoginAt,
            Title = appUser.Title,
            MaritalStatus = appUser.MaritalStatus,
            Gender = appUser.Gender,
            Nationality = appUser.Nationality,
            Dependants = appUser.Dependants,
            ResidentialAddress = appUser.ResidentialAddress,
            City = appUser.City,
            Mmda = appUser.Mmda,
            Landmark = appUser.Landmark,
            Employer = appUser.Employer,
            Sector = appUser.Sector,
            Occupation = appUser.Occupation,
            SsnitNo = appUser.SsnitNo,
            WorkAddress = appUser.WorkAddress,
            YearsWithEmployer = appUser.YearsWithEmployer
        };
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        await Task.CompletedTask;
        return _jwtTokenService.ValidateToken(token);
    }

    private async Task<TokenDto> GenerateTokensAsync(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        // Add role claims
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15), // Access token expires in 15 minutes
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        // Generate refresh token
        var refreshToken = GenerateRefreshToken();

        // Store refresh token
        await _userManager.SetAuthenticationTokenAsync(user, "RefreshToken", "Token", refreshToken);

        return new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = token.ValidTo
        };
    }

    public async Task<ApiResponse<bool>> VerifyKycAsync(string userId, KycSubmissionRequestDto request)
    {
        try
        {
            var userGuid = Guid.Parse(userId);
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == userGuid);
            
            if (user == null)
            {
                return new ApiResponse<bool> { Success = false, Message = "User not found" };
            }

            user.KycStatus = KycStatus.Verified; // For development auto-verify
            user.NationalId = request.GhCard;
            
            // For now, we don't map AccountType to Market as it's personal/business
            
            // Upload documents if provided
            if (request.GhanaCardFront != null)
            {
                var url = await _fileStorageService.UploadFileAsync(request.GhanaCardFront.OpenReadStream(), request.GhanaCardFront.FileName, request.GhanaCardFront.ContentType);
                user.Documents.Add(new UserDocument { UserId = user.Id, Type = DocumentType.IDCard, DocumentUrl = url, DocumentNumber = request.GhCard });
            }

            if (request.GhanaCardBack != null)
            {
                var url = await _fileStorageService.UploadFileAsync(request.GhanaCardBack.OpenReadStream(), request.GhanaCardBack.FileName, request.GhanaCardBack.ContentType);
                user.Documents.Add(new UserDocument { UserId = user.Id, Type = DocumentType.IDCard, DocumentUrl = url, DocumentNumber = request.GhCard });
            }

            if (request.Certificate != null)
            {
                var url = await _fileStorageService.UploadFileAsync(request.Certificate.OpenReadStream(), request.Certificate.FileName, request.Certificate.ContentType);
                user.Documents.Add(new UserDocument { UserId = user.Id, Type = DocumentType.BusinessRegistration, DocumentUrl = url });
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(default);

            return new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = "KYC verified successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during KYC verification");
            return new ApiResponse<bool> { Success = false, Message = "An error occurred during verification" };
        }
    }

    private static string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
