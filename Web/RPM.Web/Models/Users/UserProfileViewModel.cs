namespace RPM.Web.Models.Users
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using RPM.Data.Models;

    public class UserProfileViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime Birthdate { get; set; }

        public CloudImage ProfilePic { get; set; }

        [Display(Name = "Registered At")]
        public DateTime RegisteredAt { get; set; }

        public string About { get; set; }

        // Collections
        public IEnumerable<Home> Homes { get; set; }

        public IEnumerable<Rental> Rentals { get; set; }

        public IEnumerable<Rental> Rented { get; set; }

        public virtual IEnumerable<string> Roles { get; set; }
    }
}
