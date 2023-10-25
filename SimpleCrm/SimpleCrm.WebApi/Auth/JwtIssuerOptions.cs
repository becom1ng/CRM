using Microsoft.IdentityModel.Tokens;

namespace SimpleCrm.WebApi.Auth
{
    public class JwtIssuerOptions
    {
        /// <summary>
        /// "iss"
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// "sub"
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// "aud"
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// "exp"
        /// </summary>
        public DateTime Expiration => IssuedAt.Add(ValidFor);
        /// <summary>
        /// "nbf"
        /// </summary>
        public DateTime NotBefore => DateTime.UtcNow;
        /// <summary>
        /// "iat"
        /// </summary>
        public DateTime IssuedAt => DateTime.UtcNow;
        /// <summary>
        /// Set the timespan the token will be valid for (default is 120 min)
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(120);
        /// <summary>
        /// "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        public Func<Task<string>> JtiGenerator =>
          () => Task.FromResult(Guid.NewGuid().ToString());

        public SigningCredentials SigningCredentials { get; set; }
    }
}
