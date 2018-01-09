namespace MovieCatalog.Web.Areas.Admin.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Common;

    public class ChangeUserPasswordFormModel : PaginationBaseModel
    {
        public string Username { get; set; }

        [Required]
        [MinLength(GlobalConstants.PasswordMinLength, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(GlobalConstants.PasswordMaxLength, ErrorMessage = "The {0} must be max {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
