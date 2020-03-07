namespace RPM.Services.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using RPM.Data.Models;
    using RPM.Services.Mapping;


    public class AdminUserListingServiceModel : IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public List<string> UserRoles { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<User, AdminUserListingServiceModel>();
        }
    }
}
