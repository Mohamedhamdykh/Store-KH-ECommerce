namespace Store.KH.APIs.Erorrs
{
    public class ApiValidationErorrResponse : ApiErrorResponse
    {
        public ApiValidationErorrResponse() : base(400)
        {
            
        }
        public IEnumerable<string> Erorrs { get; set; } = new List<string>();
    }
}
