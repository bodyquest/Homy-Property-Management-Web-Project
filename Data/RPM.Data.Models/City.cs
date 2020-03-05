namespace RPM.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public ICollection<Home> Homes { get; set; }

    }
}
