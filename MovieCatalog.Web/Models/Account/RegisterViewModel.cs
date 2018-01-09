namespace MovieCatalog.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Common;

    public class RegisterViewModel
    {
        [Required]
        [StringLength(GlobalConstants.UsernameMaxLength, MinimumLength = GlobalConstants.UsernameMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(GlobalConstants.PasswordMaxLength, MinimumLength = GlobalConstants.PasswordMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
