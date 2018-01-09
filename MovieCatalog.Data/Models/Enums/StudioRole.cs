namespace MovieCatalog.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum StudioRole
    {
        [Display(Name = "Distributor")]
        Distributor = 0,

        [Display(Name = "Production Company")]
        ProductionCompany = 1
    }
}
