namespace Basic.WebApi.Models
{
    public class AuthResult
    {
        public AuthResult()
        {
            TokenType = "Bearer";
        }

        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public int ExpireIn { get; set; }
    }
}
