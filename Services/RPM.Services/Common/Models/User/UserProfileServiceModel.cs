namespace RPM.Services.Common.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using RPM.Data.Models;

    public class UserProfileServiceModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public DateTime Birthdate { get; set; }

        public CloudImage ProfilePic { get; set; }

        public DateTime RegisteredAt { get; set; }

        public string About { get; set; }

        // Collections
        public IEnumerable<Home> Homes { get; set; }

        public IEnumerable<string> TinyPicPubIds { get; set; }

        public IEnumerable<Rental> Rentals { get; set; }

        public IEnumerable<Rental> Rented { get; set; }

        public virtual IEnumerable<string> Roles { get; set; }
    }
}
