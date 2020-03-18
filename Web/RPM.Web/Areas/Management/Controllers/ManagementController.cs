namespace RPM.Web.Areas.Management.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Web.Controllers;

    using static RPM.Common.GlobalConstants;

    [Area(ManagementArea)]
    [Authorize(Roles = OwnerRoleName)]
    public class ManagementController : BaseController
    {
    }
}
