using Newtonsoft.Json.Linq;

namespace SimpleCrm.WebApi.Auth
{
    public class GoogleAuthVerifier<T>
    {
        private readonly GoogleAuthSettings _googleAuthSettings;
        private readonly string _host;
        private readonly ILogger<T> _logger;

        public GoogleAuthVerifier(GoogleAuthSettings googleAuthSettings, string host,
            ILogger<T> logger)
        {
            _googleAuthSettings = googleAuthSettings;
            _host = host;
            _logger = logger;
        }

        public async Task<GoogleUserProfile> AcquireUser(string token)
        {
            try
            {
                var client = new HttpClient();

                //A. build the request parameters
                var tokenRequestParameters = new Dictionary<string, string>()
                {
                    { "client_id", _googleAuthSettings.ClientId },
                    { "client_secret", _googleAuthSettings.ClientSecret },
                    { "redirect_uri", _host + "signin-google" },
                    { "code", token },
                    { "grant_type", "authorization_code" }
                };
                //B. encode the parameters for a url request
                var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
                //C. build the post request
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token");
                //"https://accounts.google.com/o/oauth2/v2/auth"

                requestMessage.Headers.Accept.Add(
                  new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                requestMessage.Content = requestContent;
                //D. now send the post request
                var response = await client.SendAsync(requestMessage);
                //E. wait for and then read the response content
                var payloadStr = await response.Content.ReadAsStringAsync();
                //F. parse the response into an object, a typeof JObject
                var payload = JObject.Parse(payloadStr);
                //{"token_type":"Bearer","scope":"openid https://www.googleapis.com/auth/userinfo.email openid","expires_in":3599,"ext_expires_in":0,"access_token":"ya...171"}

                if (payload["error"] != null)
                {   
                    var err = payload["error"];
                    _logger.LogWarning("Google token error response: {0}", payloadStr);
                    return new GoogleUserProfile
                    {
                        IsSuccessful = false,
                        Error = new OAuthError
                        {
                            Code = payload.Value<string>("error"),
                            Message = payload.Value<string>("error_description")
                        }
                    };
                }

                var graphMessage = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v2/userinfo");
                graphMessage.Headers.Add("Authorization", "Bearer " + payload["access_token"]);
                var graphResponse = await client.SendAsync(graphMessage);
                var graphPayloadStr = await graphResponse.Content.ReadAsStringAsync();
                var graphPayload = JObject.Parse(graphPayloadStr);
                //{"@odata.context":"https://graph.microsoft.com/v1.0/$metadata#users/$entity","id":"ea0...070","businessPhones":[],"displayName":"Michael Lang","givenName":"Michael","jobTitle":null,"mail":"michael.lang@nexulacademy.com","mobilePhone":null,"officeLocation":null,"preferredLanguage":null,"surname":"Lang","userPrincipalName":"michael.lang@nexulacademy.com"}

                if (graphPayload["error"] != null)
                {   
                    var err = graphPayload["error"];
                    _logger.LogWarning("Google error response: {0}", graphPayloadStr);
                    return new GoogleUserProfile
                    {
                        IsSuccessful = false,
                        Error = new OAuthError
                        {
                            Code = err.Value<string>("code"),
                            Message = err.Value<string>("message")
                        }
                    };
                }

                var profile = new GoogleUserProfile
                {
                    IsSuccessful = true,
                    //Context = graphPayload.Value<string>("@odata.context"),
                    Id = graphPayload.Value<string>("id"),
                    //BusinessPhones = graphPayload.Value<string[]>("businessPhones"),
                    DisplayName = graphPayload.Value<string>("name"),
                    GivenName = graphPayload.Value<string>("given_name"),
                    //JobTitle = graphPayload.Value<string>("jobTitle"),
                    Mail = graphPayload.Value<string>("email"),
                    MobilePhone = graphPayload.Value<string>("mobilePhone"),
                    //OfficeLocation = graphPayload.Value<string>("officeLocation"),
                    PreferredLanguage = graphPayload.Value<string>("locale"),
                    Surname = graphPayload.Value<string>("family_name"),
                    UserPrincipalName = graphPayload.Value<string>("email")
                };
                if (string.IsNullOrWhiteSpace(profile.Mail))
                {
                    profile.Mail = profile.UserPrincipalName;
                }
                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: {0} Details: {1}", ex, ex.StackTrace);
                throw;
            }
        }
    }

    public class GoogleUserProfile : IOAuthUserProfile
    {
        public bool IsSuccessful { get; set; }
        public OAuthError Error { get; set; }
        //public string Context { get; set; }
        public string Id { get; set; }
        //public string[] BusinessPhones { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string JobTitle { get; set; }
        public string Mail { get; set; }
        public string MobilePhone { get; set; }
        //public string OfficeLocation { get; set; }
        public string PreferredLanguage { get; set; }
        public string Surname { get; set; }
        public string UserPrincipalName { get; set; }
    }
}
