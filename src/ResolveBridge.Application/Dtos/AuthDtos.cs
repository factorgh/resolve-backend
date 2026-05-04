namespace ResolveBridge.Application.Dtos;

public class AuthResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = null!;
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NationalId { get; set; }
    public string Market { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string KycStatus { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool EmailVerified { get; set; }
    public bool PhoneVerified { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // Institutional Details
    public string? Title { get; set; }
    public string? MaritalStatus { get; set; }
    public string? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? Dependants { get; set; }
    public string? ResidentialAddress { get; set; }
    public string? City { get; set; }
    public string? Mmda { get; set; }
    public string? Landmark { get; set; }
    public string? Employer { get; set; }
    public string? Sector { get; set; }
    public string? Occupation { get; set; }
    public string? SsnitNo { get; set; }
    public string? WorkAddress { get; set; }
    public string? YearsWithEmployer { get; set; }
}

public class RegisterRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Market { get; set; } = string.Empty;
    
    // Onboarding fields
    public List<string> Goals { get; set; } = new List<string>();
    public DateTime? DateOfBirth { get; set; }
    public string? IdType { get; set; }
    public string? IdNumber { get; set; }
    public string? EmploymentStatus { get; set; }
    public string? MonthlyIncome { get; set; }
    public string? LoanDuration { get; set; }
    
    // New Institutional fields
    public string? Title { get; set; }
    public string? MaritalStatus { get; set; }
    public string? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? Dependants { get; set; }
    public string? ResidentialAddress { get; set; }
    public string? City { get; set; }
    public string? Mmda { get; set; }
    public string? Landmark { get; set; }
    public string? Employer { get; set; }
    public string? Sector { get; set; }
    public string? Occupation { get; set; }
    public string? SsnitNo { get; set; }
    public string? WorkAddress { get; set; }
    public string? YearsWithEmployer { get; set; }
}

public class LoginRequestDto
{
    public string Identifier { get; set; } = string.Empty; // Can be email or phone number
    public string Password { get; set; } = string.Empty;
}

public class PhoneLoginRequestDto
{
    public string PhoneNumber { get; set; } = string.Empty;
}

public class VerifyOtpRequestDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
}

public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class ForgotPasswordRequestDto
{
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class ChangePasswordRequestDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class UpdateProfileRequestDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NationalId { get; set; }
    public string? Title { get; set; }
    public string? MaritalStatus { get; set; }
    public string? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? Dependants { get; set; }
    public string? ResidentialAddress { get; set; }
    public string? City { get; set; }
    public string? Mmda { get; set; }
    public string? Landmark { get; set; }
    public string? Employer { get; set; }
    public string? Sector { get; set; }
    public string? Occupation { get; set; }
    public string? SsnitNo { get; set; }
    public string? WorkAddress { get; set; }
    public string? YearsWithEmployer { get; set; }
}



public class VerifyOtpResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ForgotPasswordResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ResetPasswordResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; }
}

public class TokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
