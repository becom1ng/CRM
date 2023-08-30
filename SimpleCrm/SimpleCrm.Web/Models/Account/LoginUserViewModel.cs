using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SimpleCrm.Web.Models.Account
{
    public class LoginUserViewModel
    {
        [Required, MaxLength(256), DataType(DataType.EmailAddress), DisplayName("Email")]
        public string UserName { get; set; }

        [Required, MaxLength(256), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}