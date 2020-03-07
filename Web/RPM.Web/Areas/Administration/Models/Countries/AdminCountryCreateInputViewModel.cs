namespace RPM.Web.Areas.Administration.Models.Countries
{
    using System.ComponentModel.DataAnnotations;

    using static RPM.Common.GlobalConstants;

    public class AdminCountryCreateInputViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(CountryCodeLength, MinimumLength = CountryCodeLength)]
        public string Code { get; set; }
    }
}
