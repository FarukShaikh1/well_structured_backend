namespace FMS_Collection.Core.Common
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; } = default;
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 200;

        public static ServiceResponse<T> Ok(T data, string message) =>
            new() { Success = true, Data = data, Message = message, StatusCode = 200 };

        public static ServiceResponse<T> Fail(string message, int statusCode = 400) =>
            new() { Success = false, Data = default, Message = message, StatusCode = statusCode };
    }
}
