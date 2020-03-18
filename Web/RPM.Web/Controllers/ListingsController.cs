namespace RPM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class ListingsController : BaseController
    {
        public ListingsController()
        {

        }

        [ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(string id)
        {
            return this.View();
        }
    }
}
