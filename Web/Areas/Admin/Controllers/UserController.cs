using Core.Users.Commands.EditUserRoles;
using Core.Users.Queries.GetAllUsers;
using Core.Users.Queries.IsUserAdmin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> ManageUsers()
        {
            IEnumerable<ListUserModel> users = await mediator.Send(new GetAllUsersQuery());

            return View(users);
        }

        public async Task<IActionResult> EditRoles(string id, string[] roles)
        {
            await mediator.Send(new EditUserRolesCommand(id, roles));
            bool isUserAdmin = await mediator.Send(new IsUserAdminQuery());

            return Ok(isUserAdmin);
        }
    }
}
