namespace RPM.Web.Areas.Administration.Models.Listings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CloudImageViewModel
    {
        public int Id { get; set; }

        public string PicturePublicId { get; set; }

        public string PictureUrl { get; set; }
    }
}
