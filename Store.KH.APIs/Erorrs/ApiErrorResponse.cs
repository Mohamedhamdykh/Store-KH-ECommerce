namespace Store.KH.APIs.Erorrs
{
    public class  ApiErrorResponse
    {
        public ApiErrorResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            var message = statusCode switch
            {
                400 => "a bad Requset , You Have Made",
                401 => "Authorized, You Are Not",
                404 => "Resource was Not Found",
                500 => "Server Erorr",
                _ => null
            };



            return message;
        }
    }
}
