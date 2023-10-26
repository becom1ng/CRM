namespace SimpleCrm.WebApi.Models.Auth
{
    public class UserSummaryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string JwtToken { get; set; }
        public string[] Roles { get; set; } // TODO? Create a class, possibly enum like CustomerType
        public int AccountId { get; set; }
        // TODO? Add any other useful props
    }
}
