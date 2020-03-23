namespace RPM.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class City
    {
        public City()
        {
            this.Homes = new HashSet<Home>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public ICollection<Home> Homes { get; set; }
    }
}
