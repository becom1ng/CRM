namespace SimpleCrm.WebApi.Models.Auth
{
    public class GoogleAuthViewModel
    { // This contains what we should receive from the Google endpoint (look in the URL that comes back)
        public string AccessToken { get; set; }
        public string BaseHref { get; set; }
        public string State { get; set; }
    }
}
