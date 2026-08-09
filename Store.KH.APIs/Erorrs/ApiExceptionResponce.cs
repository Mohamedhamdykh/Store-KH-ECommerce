namespace Store.KH.APIs.Erorrs
{
    public class ApiExceptionResponce : ApiErrorResponse
    {
        public ApiExceptionResponce(int statusCode , string? message = null , string? details = null)
            : base(statusCode , message)
        {
            Details = details;
        }
        public string? Details { get; set; }
    }
}
