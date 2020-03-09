using System.ComponentModel.DataAnnotations;

namespace RPM.Services.Admin.Models
{
    public class AdminCitiesListingServiceModel
    {
        public int Id { get; set; }

        [Display(Name = "City Name")]
        public string Name { get; set; }

        [Display(Name = "Country Name")]
        public string Country { get; set; }
    }
}
