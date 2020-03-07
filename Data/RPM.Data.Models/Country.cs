namespace RPM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using static RPM.Common.GlobalConstants;

    public class Country
    {
        public Country()
        {
            this.Cities = new HashSet<City>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(CountryCodeLength)]
        public string Code { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
