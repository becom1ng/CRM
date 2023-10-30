using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SimpleCrm.WebApi.Models.Auth
{
    public class RegisterUserViewModel
    {
        [Required, MaxLength(256), DisplayName("Name")]
        public string Name { get; set; }

        [Required, MaxLength(256), DataType(DataType.EmailAddress), DisplayName("Email")]
        public string EmailAddress { get; set; }

        [Required, MaxLength(256), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, MaxLength(256), DataType(DataType.Password), DisplayName("Confirm Password"), Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}