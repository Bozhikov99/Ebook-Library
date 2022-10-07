using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleConstants.Administrator)]
    [Area(RoleConstants.AdminArea)]
    public class BaseController : Controller
    {
    }
}
