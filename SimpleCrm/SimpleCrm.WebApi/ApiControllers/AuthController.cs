using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleCrm.WebApi.Auth;
using SimpleCrm.WebApi.Models.Auth;

namespace SimpleCrm.WebApi.ApiControllers
{

    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<CrmUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly MicrosoftAuthSettings _microsoftAuthSettings;
        private readonly GoogleAuthSettings _googleAuthSettings;

        public AuthController(UserManager<CrmUser> userManager, IJwtFactory jwtFactory, IConfiguration configuration, ILogger<AuthController> logger, IOptions<MicrosoftAuthSettings> microsoftAuthSettings, IOptions<GoogleAuthSettings> googleAuthSettings)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _configuration = configuration;
            _logger = logger;
            _microsoftAuthSettings = microsoftAuthSettings.Value;
            _googleAuthSettings = googleAuthSettings.Value;
        }

        [HttpGet("external/microsoft")]
        public IActionResult GetMicrosoft()
        {   // only needed for the client to know what to send to Microsoft on the front-end redirect to login with Microsoft
            return Ok(new
            {   //this is the public application id, don't return the secret 'Password' here!
                client_id = _microsoftAuthSettings.ClientId,
                scope = "https://graph.microsoft.com/user.read",
                state = "" //arbitrary state to return again for this user
            });
        }

        [HttpPost("external/microsoft")]
        public async Task<IActionResult> PostMicrosoft([FromBody] MicrosoftAuthViewModel model)
        {
            var verifier = new MicrosoftAuthVerifier<AuthController>(_microsoftAuthSettings, _configuration["HttpHost"] + (model.BaseHref ?? "/"), _logger);
            var profile = await verifier.AcquireUser(model.AccessToken);

            // validate the 'profile' object is successful, and email address is included
            if (!profile.IsSuccessful)
            {
                return BadRequest(); // 400 - TODO? Provide an error message.
            }
            if (String.IsNullOrWhiteSpace(profile.Mail)) 
            {
                return Forbid("Email address not available from provider."); // 403
            }

            // verify UserLoginInfo
            var info = new UserLoginInfo("Microsoft", profile.Id, "Microsoft");
            if (info == null || String.IsNullOrWhiteSpace(info.ProviderKey))
            {
                return BadRequest(); // 400 - TODO? Provide an error message.
            }

            // ready to create the LOCAL user account (if necessary) and jwt
            var user = await _userManager.FindByEmailAsync(profile.Mail);
            if (user == null)
            {
                // create a new user
                var newUser = new CrmUser
                {
                    UserName = profile.Mail,
                    Email = profile.Mail,
                    DisplayName = profile.DisplayName,
                    PhoneNumber = profile.MobilePhone
                };

                // generate password - #1aA ensures all required character types will be in the random password
                var password = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8) + "#1aA";
                var createResult = await _userManager.CreateAsync(newUser, password);
                if (!createResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Could not create user.");
                }

                // final verification that new local user can be found
                user = await _userManager.FindByEmailAsync(profile.Mail);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Failed to create local user account.");
                }
            }

            var userModel = await GetUserData(user);
            return Ok(userModel);
        }

        [HttpGet("external/google")]
        public IActionResult GetGoogle()
        {   // only needed for the client to know what to send to Google on the front-end redirect to login with Google
            return Ok(new
            {   //this is the public application id, don't return the secret 'Password' here!
                client_id = _googleAuthSettings.ClientId,
                scope = "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email openid",
                state = "" //arbitrary state to return again for this user
            });
        }

        // ****** TODO: Set this up for Google with JWT.
        [HttpPost("external/google")]
        public async Task<IActionResult> PostGoogle([FromBody] GoogleAuthViewModel model)
        {
            var verifier = new GoogleAuthVerifier<AuthController>(_googleAuthSettings, _configuration["HttpHost"] + (model.BaseHref ?? "/"), _logger);
            var profile = await verifier.AcquireUser(model.AccessToken);

            // validate the 'profile' object is successful, and email address is included
            if (!profile.IsSuccessful)
            {
                return BadRequest(); // 400 - TODO? Provide an error message.
            }
            if (String.IsNullOrWhiteSpace(profile.Mail))
            {
                return Forbid("Email address not available from provider."); // 403
            }

            // verify UserLoginInfo
            var info = new UserLoginInfo("Google", profile.Id, "Google");
            if (info == null || String.IsNullOrWhiteSpace(info.ProviderKey))
            {
                return BadRequest(); // 400 - TODO? Provide an error message.
            }

            // ready to create the LOCAL user account (if necessary) and jwt
            var user = await _userManager.FindByEmailAsync(profile.Mail);
            if (user == null)
            {
                // create a new user
                var newUser = new CrmUser
                {
                    UserName = profile.Mail,
                    Email = profile.Mail,
                    DisplayName = profile.DisplayName,
                    PhoneNumber = profile.MobilePhone
                };

                // generate password - #1aA ensures all required character types will be in the random password
                var password = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8) + "#1aA";
                var createResult = await _userManager.CreateAsync(newUser, password);
                if (!createResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Could not create user.");
                }

                // final verification that new local user can be found
                user = await _userManager.FindByEmailAsync(profile.Mail);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Failed to create local user account.");
                }
            }

            var userModel = await GetUserData(user);
            return Ok(userModel);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var user = await Authenticate(credentials.EmailAddress, credentials.Password);
            if (user == null) 
                return Unauthorized("Invalid username or password.");

            var userModel = await GetUserData(user);
            // returns a UserSummaryViewModel containing a JWT and other user info
            return Ok(userModel);
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var user = new CrmUser
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress,
                DisplayName = model.Name,
            };
            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Could not create user.");
            }

            // final verification that new local user can be found and authd
            var userAuth = await Authenticate(model.EmailAddress, model.Password);
            if (userAuth == null)
                return Unauthorized("Invalid username or password.");

            var userModel = await GetUserData(userAuth);
            _logger.LogInformation("User created a new account with password.");
            // returns a UserSummaryViewModel containing a JWT and other user info
            return Ok(userModel);
        }

        /// <summary>
        /// Endpoint to refresh/regenerate JWT token if user stays logged in.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "ApiUser")] // policy created in startup.cs
        [HttpPost("verify")] // POST api/auth/verify
        public async Task<IActionResult> Verify()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Claims.Single(c => c.Type == "id");
                var user = _userManager.Users.FirstOrDefault(x => x.Id.ToString() == userId.Value);
                if (user == null)
                    return Forbid();

                var userModel = await GetUserData(user);
                return new ObjectResult(userModel);
            }

            return Forbid();
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
