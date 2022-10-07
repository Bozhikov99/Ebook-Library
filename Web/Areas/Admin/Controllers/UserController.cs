using Core.Commands.UserCommands;
using Core.Queries.User;
using Core.ViewModels.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IMediator mediator;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(IMediator mediator,RoleManager<IdentityRole> roleManager)
        {
            this.mediator = mediator;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> ManageUsers()
        {
            IEnumerable<ListUserModel> users = await mediator.Send(new GetAllUsersQuery());

            return View(users);
        }

        public async Task<IActionResult> EditRoles(string id, string[] roles)
        {
            await mediator.Send(new EditRolesCommand(id, roles));
            bool isUserAdmin = await mediator.Send(new IsUserAdminQuery());

            return Ok(isUserAdmin);
        }
    }
}
