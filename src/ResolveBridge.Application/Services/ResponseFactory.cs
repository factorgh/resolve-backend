using ResolveBridge.Application.Common;
using ResolveBridge.Application.Interfaces;

namespace ResolveBridge.Application.Services;

public class ResponseFactory : IResponseFactory
{
    public ApiResponse<T> Success<T>(T data, string message = "Success", PaginationInfo? pagination = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Pagination = pagination
        };
    }

    public ApiResponse<T> Error<T>(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string> { message }
        };
    }

    public ApiResponse<T> NotFound<T>(string message = "Resource not found")
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = new List<string> { message }
        };
    }

    public ApiResponse<T> Unauthorized<T>(string message = "Unauthorized access")
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = new List<string> { message }
        };
    }
}
