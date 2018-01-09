namespace MovieCatalog.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum DiscLayersType
    {
        [Display(Name = "Single Side - Single Layer")]
        SingleSideSingleLayer = 0,

        [Display(Name = "Single Side - Dual Layer")]
        SingleSideDualLayer = 1,

        [Display(Name = "Dual Side - Single Layer")]
        DualSideSingleLayer = 2,

        [Display(Name = "Dual Side - Dual Layer")]
        DualSideDualLayer = 3
    }
}
