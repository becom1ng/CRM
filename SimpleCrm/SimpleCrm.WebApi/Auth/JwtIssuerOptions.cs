using Microsoft.IdentityModel.Tokens;

namespace SimpleCrm.WebApi.Auth
{
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
        public DateTime ExpirationTime { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}
