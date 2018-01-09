namespace MovieCatalog.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum CrewRole
    {
        [Display(Name = "Director")]
        Director = 0,

        [Display(Name = "Writer")]
        Writer = 1,

        [Display(Name = "Producer")]
        Producer = 2,

        [Display(Name = "Original Music")]
        Composer = 3,

        [Display(Name = "Cinematography")]
        Cinematographer = 4,

        [Display(Name = "Production Design")]
        Production_Designer = 5
    }
}
