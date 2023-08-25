using System.ComponentModel.DataAnnotations;

namespace SimpleCrm.Web.Models.Home
{
    public class CustomerEditViewModel
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        [Required()]
        [MinLength(1), MaxLength(40)]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required()]
        [MinLength(1), MaxLength(60)]
        public string LastName { get; set; }
        [Display(Name = "Phone Number")]
        [Required()]
        [DataType(DataType.PhoneNumber)]
        [MinLength(7), MaxLength(12)]
        public string PhoneNumber { get; set; }
        [Display(Name = "Sign up for newsletter?")]
        public bool OptInNewsletter { get; set; }
        [Display(Name = "Customer Type")]
        public CustomerType Type { get; set; }
    }
}