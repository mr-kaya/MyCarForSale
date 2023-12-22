using System.Text.Json.Serialization;

namespace MyCarForSale.Core.DTOs;

public class CustomResponseDto<T>
{
    public T Data { get; set; }
    [JsonIgnore]
    public int StatusCode { get; set; }
    public List<String> Erorrs { get; set; }
    
    public static CustomResponseDto<T> Success(int statusCode, T data)
    {
        return new CustomResponseDto<T>() { Data = data, StatusCode = statusCode };
    }

    public static CustomResponseDto<T> Fail(int statusCode, List<string> errors)
    {
        return new CustomResponseDto<T>() { StatusCode = statusCode, Erorrs = errors };
    }

    public static CustomResponseDto<T> Fail(int statusCode, string error)
    {
        return new CustomResponseDto<T>() { StatusCode = statusCode, Erorrs = new List<string>() { error } };
    }
}

public class CustomNoContentResponseDto
{
    public int StatusCode { get; set; }
    public List<String> Errors { get; set; }
    
    public static CustomNoContentResponseDto Success(int statusCode)
    {
        return new CustomNoContentResponseDto() { StatusCode = statusCode };
    }       

    public static CustomNoContentResponseDto Fail(int statusCode, List<string> errors)
    {
        return new CustomNoContentResponseDto() { StatusCode = statusCode, Errors = errors };
    }

    public static CustomNoContentResponseDto Fail(int statusCode, string error)
    {
        return new CustomNoContentResponseDto() { StatusCode = statusCode, Errors = new List<string>() { error } };
    }
}