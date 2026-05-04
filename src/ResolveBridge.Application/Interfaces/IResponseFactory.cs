using ResolveBridge.Application.Common;

namespace ResolveBridge.Application.Interfaces;

public interface IResponseFactory
{
    ApiResponse<T> Success<T>(T data, string message = "Success", PaginationInfo? pagination = null);
    ApiResponse<T> Error<T>(string message, List<string>? errors = null);
    ApiResponse<T> NotFound<T>(string message = "Resource not found");
    ApiResponse<T> Unauthorized<T>(string message = "Unauthorized access");
}
