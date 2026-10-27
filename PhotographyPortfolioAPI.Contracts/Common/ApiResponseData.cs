namespace PhotographyPortfolioAPI.Contracts.Common
{
    public class ApiResponseData<T>
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
        public object Id { get; set; }

        public static ApiResponseData<T> Success(T data = default, string message = "Success", object id = null, int statusCode = 200)
        {
            return new ApiResponseData<T>
            {
                StatusCode = statusCode,
                StatusMessage = $"{statusCode} {message}",
                ErrorMessage = null,
                Data = data,
                Id = id
            };
        }

        public static ApiResponseData<T> Error(string errorMessage, int statusCode = 500)
        {
            return new ApiResponseData<T>
            {
                StatusCode = statusCode,
                StatusMessage = $"{statusCode} Error",
                ErrorMessage = errorMessage,
                Data = default,
                Id = null
            };
        }
    }
}
