using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleCrm.WebApi.Auth;
using SimpleCrm.WebApi.Models.Auth;

namespace SimpleCrm.WebApi.ApiControllers
{

    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<CrmUser> _userManager;
        private readonly IJwtFactory _jwtFactory;

        public AuthController(UserManager<CrmUser> userManager, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var user = await Authenticate(credentials.EmailAddress, credentials.Password);
            if (user == null) 
                return UnprocessableEntity("Invalid username or password.");

            var userModel = await GetUserData(user);
            // returns a UserSummaryViewModel containing a JWT and other user info
            return Ok(userModel);
        }

        private async Task<CrmUser> Authenticate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return await Task.FromResult<CrmUser>(null);

            // get the user
            var userToVerify = await _userManager.FindByEmailAsync(email); // TODO? Set identity option: require unique email
            if (userToVerify == null) 
                return await Task.FromResult<CrmUser>(null);

            // check the password for the user
            if (await _userManager.CheckPasswordAsync(userToVerify, password)) 
                return await Task.FromResult(userToVerify);

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<CrmUser>(null);
        }

        private async Task<UserSummaryViewModel> GetUserData(CrmUser user)
        {
            if (user == null) return null;

            // generate the jwt for the local user
            var jwt = await _jwtFactory.GenerateEncodedToken(user.UserName,
                _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id.ToString()));

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0) { roles.Add("prospect"); }

            var userModel = new UserSummaryViewModel
            {   //JWT could inject all these properties instead of creating a model,
                //but a model is a little easier to access from client code without
                //decoding the token. When this user model starts to contain arrays
                //of complex data, including it all in the JWT value can get complicated.
                Id = user.Id,
                Name = user.DisplayName,
                EmailAddress = user.Email,
                JwtToken = jwt,
                Roles = roles.ToArray(), //each role could be a separate claim in the JWT
                AccountId = 0 //TODO: load this from registration data
            };

            return userModel;
        }
    }
}
