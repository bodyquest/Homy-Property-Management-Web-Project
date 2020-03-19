namespace RPM.Services.Common.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static RPM.Common.GlobalConstants;

    public class UserDetailsInputModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [MaxLength(AboutMaxLength)]
        public string About { get; set; }
    }
}
