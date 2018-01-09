namespace MovieCatalog.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum DiscFormatType
    {
        [Display(Name = "Blu-ray")]
        BRD = 0,

        [Display(Name = "DVD")]
        DVD = 1,

        [Display(Name = "Ultra HD")]
        UHD = 2
    }
}
