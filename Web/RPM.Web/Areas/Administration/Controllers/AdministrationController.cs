namespace RPM.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Web.Controllers;

    using static RPM.Common.GlobalConstants;

    [Area(AdminArea)]
    [Authorize(Roles = AdministratorRoleName)]
    public class AdministrationController : BaseController
    {
    }
}
