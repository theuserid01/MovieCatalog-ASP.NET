namespace MovieCatalog.Web.Areas.Admin.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Common;

    public class ChangeUserDetailsFormModel : PaginationBaseModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [MinLength(GlobalConstants.UsernameMinLength, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(GlobalConstants.UsernameMaxLength, ErrorMessage = "The {0} must be {1} max characters long.")]
        [Display(Name = "Username")]
        public string Username { get; set; }
    }
}
