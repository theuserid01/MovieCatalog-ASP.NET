namespace MovieCatalog.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum ContentRatingType
    {
        [Display(Name = "NR", Description = "NOT RATED")]
        NR = 0,

        [Display(Name = "G", Description = "GENERAL AUDIENCES")]
        USG = 1,

        [Display(Name = "PG", Description = "PARENTAL GUIDANCE SUGGESTED")]
        USPG = 2,

        [Display(Name = "PG-13", Description = "PARENTS STRONGLY CAUTIONED")]
        USPG13 = 3,

        [Display(Name = "R", Description = "RESTRICTED")]
        USR = 4,

        [Display(Name = "NC-17", Description = "ADULTS ONLY")]
        USNC17 = 5,

        [Display(Name = "U", Description = "SUITABLE FOR ALL")]
        UKU = 6,

        [Display(Name = "PG", Description = "PARENTAL GUIDANCE")]
        UKPG = 7,

        [Display(Name = "12A", Description = "SUITABLE FOR 12 YEARS AND OVER")]
        UK12A = 8,

        [Display(Name = "12", Description = "SUITABLE FOR 12 YEARS AND OVER")]
        UK12 = 9,

        [Display(Name = "15", Description = "SUITABLE ONLY FOR 15 YEARS OR OVER")]
        UK15 = 10,

        [Display(Name = "18", Description = "SUITABLE ONLY FOR 18 YEARS OR OVER")]
        UK18 = 11,

        [Display(Name = "FSK 0", Description = "RELEASED WITHOUT AGE RESTRICTION")]
        DEFSK0 = 12,

        [Display(Name = "FSK 6", Description = "RELEASED TO AGES 6 OR OLDER")]
        DEFSK6 = 13,

        [Display(Name = "FSK 12", Description = "RELEASED TO AGES 12 OR OLDER")]
        DEFSK12 = 14,

        [Display(Name = "FSK 16", Description = "RELEASED TO AGES 16 OR OLDER")]
        DEFSK16 = 15,

        [Display(Name = "FSK 18", Description = "RELEASED TO AGES 18 OR OLDER")]
        DEFSK18 = 16,

        [Display(Name = "U", Description = "RELEASED WITHOUT AGE RESTRICTION")]
        FRU = 17,

        [Display(Name = "12", Description = "RELEASED TO AGES 12 OR OLDER")]
        FR12 = 18,

        [Display(Name = "16", Description = "RELEASED TO AGES 16 OR OLDER")]
        FR16 = 19,

        [Display(Name = "18", Description = "RELEASED TO AGES 18 OR OLDER")]
        FR18 = 20
    }
}
