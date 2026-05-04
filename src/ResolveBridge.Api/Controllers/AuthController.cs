namespace ResolveBridge.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService, IResponseFactory responseFactory) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IResponseFactory _responseFactory = responseFactory;

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);

        if (!result.Success)
            return BadRequest(_responseFactory.Error<AuthResponseDto>(result.Message, result.Errors));

        return Ok(_responseFactory.Success(result, "Registration successful"));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        if (!result.Success)
            return Unauthorized(_responseFactory.Unauthorized<AuthResponseDto>(result.Message));

        return Ok(_responseFactory.Success(result, "Login successful"));
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(_responseFactory.Unauthorized<UserDto>("User not authenticated"));

        try
        {
            var user = await _authService.GetCurrentUserAsync(userId);
            return Ok(_responseFactory.Success(user, "User profile fetched successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(_responseFactory.NotFound<UserDto>("User profile not found"));
        }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);

        if (!result.Success)
            return Unauthorized(new ProblemDetails { Title = "Token Refresh Failed", Detail = result.Message });

        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Logout()
    {
        // For JWT, server-side logout usually involves token blacklisting.
        // For now, we return success as the client handles token removal.
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        var result = await _authService.ForgotPasswordAsync(request);

        if (!result.Success)
            return BadRequest(new ProblemDetails { Title = "Request Failed", Detail = result.Message });

        return Ok(result);
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        var result = await _authService.ResetPasswordAsync(request);

        if (!result.Success)
            return BadRequest(new ProblemDetails { Title = "Reset Failed", Detail = result.Message });

        return Ok(result);
    }

    [HttpPost("phone/verify-otp")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(VerifyOtpResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VerifyOtpResponseDto>> VerifyPhoneOtp([FromBody] VerifyOtpRequestDto request)
    {
        var result = await _authService.VerifyOtpAsync(request);

        if (!result.Success)
            return Unauthorized(new ProblemDetails { Title = "Verification Failed", Detail = result.Message });

        return Ok(result);
    }
    [HttpPost("verify-kyc")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<bool>>> VerifyKyc([FromForm] KycSubmissionRequestDto request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(_responseFactory.Unauthorized<bool>("User not authenticated"));

        var result = await _authService.VerifyKycAsync(userId, request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
