using System.ComponentModel.DataAnnotations;

namespace RPM.Services.Admin.Models
{
    public class AdminCountriesListingServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Country Code")]
        public string Code { get; set; }
    }
}
