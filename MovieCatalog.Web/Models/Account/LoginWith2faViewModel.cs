namespace MovieCatalog.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class LoginWith2faViewModel
    {
        [Required]
        [StringLength(7, MinimumLength = 6, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
}
