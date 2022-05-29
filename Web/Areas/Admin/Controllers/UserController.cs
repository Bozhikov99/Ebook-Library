using Core.Services.Contracts;
using Core.ViewModels.User;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Web.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(IUserService userService, RoleManager<IdentityRole> roleManager)
        {
            this.userService = userService;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> ManageUsers()
        {
            IEnumerable<ListUserModel> users = await userService.GetAll();

            return View(users);
        }

        public async Task<IActionResult> CreateRole()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Administrator"
            });

            return Ok();
        }
    }
}
