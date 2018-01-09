namespace MovieCatalog.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using MovieCatalog.Common;
    using MovieCatalog.Data.Models;
    using MovieCatalog.Services.Data.Contracts;
    using MovieCatalog.Web.Areas.Admin.Models.Users;
    using MovieCatalog.Web.Infrastructure.Extensions;
    using MovieCatalog.Web.Models.Account;

    public class UsersController : AdminBaseController
    {
        private const string UserDeletedMessage = "User {0} has been successfully deleted.";
        private const string UserProfileUpdatedMessage = "{0} profile has been successfully updated.";
        private const string UserRegisteredMessage = "User {0} has been successfully registered.";

        private readonly IAdminService adminService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public UsersController(
            IAdminService adminService,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            this.adminService = adminService;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        #region Index

        public async Task<IActionResult> Index(int page = 1, string search = null)
        {
            var viewModel = new UsersPaginationModel();
            viewModel.CurrentPage = page;
            viewModel.Search = search;

            int countUsers = await this.adminService.CountUsers(viewModel.Search);
            viewModel.TotalPages = (int)Math.Ceiling(countUsers / (double)GlobalConstants.PageSize);
            viewModel.Users = this.adminService.GetAllUsers(viewModel.CurrentPage, viewModel.Search);

            return View(viewModel);
        }

        #endregion Index

        #region ChangeUserDetails HttpGet

        public async Task<IActionResult> ChangeUserDetails(string id, int page = 1, string search = null)
        {
            User user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var formModel = new ChangeUserDetailsFormModel()
            {
                CurrentPage = page,
                Search = search,
                Email = user.Email,
                Username = user.UserName
            };

            return View(formModel);
        }

        #endregion ChangeUserDetails HttpGet

        #region ChangeUserDetails HttpPost

        [HttpPost]
        public async Task<IActionResult> ChangeUserDetails(string id, ChangeUserDetailsFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            User user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            bool emailIsChanged = formModel.Email != user.Email;
            bool usernameIsChanged = formModel.Username != user.UserName;

            if (emailIsChanged)
            {
                IdentityResult setEmailResult = await this.userManager.SetEmailAsync(user, formModel.Email);
                if (!setEmailResult.Succeeded)
                {
                    this.AddModelErrors(setEmailResult);

                    return View(formModel);
                }
            }

            if (usernameIsChanged)
            {
                IdentityResult setUsernameResult = await this.userManager.SetUserNameAsync(user, formModel.Username);
                if (!setUsernameResult.Succeeded)
                {
                    this.AddModelErrors(setUsernameResult);

                    return View(formModel);
                }
            }

            if (emailIsChanged || usernameIsChanged)
            {
                TempData.AddSuccessMessage(string.Format(UserProfileUpdatedMessage, user.UserName));
            }

            return RedirectToAction(nameof(Index), new { page = formModel.CurrentPage, search = formModel.Search });
        }

        #endregion ChangeUserDetails HttpPost

        #region ChangeUserPassword HttpGet

        public async Task<IActionResult> ChangeUserPassword(string id, int page = 1, string search = null)
        {
            User user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var formModel = new ChangeUserPasswordFormModel()
            {
                CurrentPage = page,
                Search = search,
                Username = user.UserName
            };

            return View(formModel);
        }

        #endregion ChangeUserPassword HttpGet

        #region ChangeUserPassword HttpPost

        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword(string id, ChangeUserPasswordFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            User user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            string token = await this.userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await this.userManager.ResetPasswordAsync(user, token, formModel.Password);

            if (!result.Succeeded)
            {
                this.AddModelErrors(result);

                return View(formModel);
            }

            TempData.AddSuccessMessage(string.Format(UserProfileUpdatedMessage, user.UserName));

            return RedirectToAction(nameof(Index), new { page = formModel.CurrentPage, search = formModel.Search });
        }

        #endregion ChangeUserPassword HttpPost

        #region ChangeUserRoles HttpGet

        public async Task<IActionResult> ChangeUserRoles(string id, int page = 1, string search = null)
        {
            User user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var formModel = await GetChangeUserRolesFormModel(user, page, search);

            return View(formModel);
        }

        #endregion ChangeUserRoles HttpGet

        #region ChangeUserRoles HttpPost

        [HttpPost]
        public async Task<IActionResult> ChangeUserRoles(string id, ChangeUserRolesFormModel formModel)
        {
            User user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            IEnumerable<string> availableRoles = formModel.AvailableRoles;
            IEnumerable<string> selectedRoles = formModel.SelectedRoles;

            if (availableRoles.Count() == 0 && selectedRoles.Count() == 0)
            {
                return BadRequest();
            }

            foreach (string role in selectedRoles)
            {
                bool roleExists = await this.roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    return BadRequest();
                }
            }

            IEnumerable<string> currentRoles = await this.userManager.GetRolesAsync(user);

            IdentityResult resultClearRoles = await this.userManager
                .RemoveFromRolesAsync(user, currentRoles);

            if (!resultClearRoles.Succeeded)
            {
                this.AddModelErrors(resultClearRoles);

                return View(await GetChangeUserRolesFormModel(user, formModel.CurrentPage, formModel.Search));
            }

            if (selectedRoles.Count() == 0)
            {
                TempData.AddSuccessMessage(string.Format(UserProfileUpdatedMessage, user.UserName));

                return RedirectToAction(nameof(Index), new { page = formModel.CurrentPage, search = formModel.Search });
            }

            IdentityResult resultSetRoles = await this.userManager.AddToRolesAsync(user, selectedRoles);

            if (!resultSetRoles.Succeeded)
            {
                this.AddModelErrors(resultSetRoles);

                return View(await GetChangeUserRolesFormModel(user, formModel.CurrentPage, formModel.Search));
            }

            TempData.AddSuccessMessage(string.Format(UserProfileUpdatedMessage, user.UserName));

            return RedirectToAction(nameof(Index), new { page = formModel.CurrentPage, search = formModel.Search });
        }

        #endregion ChangeUserRoles HttpPost

        #region DeleteUser HttpGet

        public async Task<IActionResult> DeleteUser(string id, int page = 1, string search = null)
        {
            User user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var formModel = new DeleteUserFormModel()
            {
                CurrentPage = page,
                Email = user.Email,
                Search = search,
                Username = user.UserName
            };

            return View(formModel);
        }

        #endregion DeleteUser HttpGet

        #region DeleteUser HttpPost

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id, DeleteUserFormModel formModel)
        {
            User user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await this.userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                this.AddModelErrors(result);

                return View(formModel);
            }

            TempData.AddSuccessMessage(string.Format(UserDeletedMessage, user.UserName));

            return RedirectToAction(nameof(Index), new { page = formModel.CurrentPage, search = formModel.Search });
        }

        #endregion DeleteUser HttpPost

        #region RegitserUser HttpGet

        public IActionResult RegisterUser()
        {
            return View("~/Views/Account/Register.cshtml");
        }

        #endregion RegitserUser HttpGet

        #region RegisterUser HttpPost

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterViewModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            IdentityResult result = await this.userManager.CreateAsync(new User()
            {
                Email = formModel.Email,
                UserName = formModel.Username
            }, formModel.Password);

            if (!result.Succeeded)
            {
                this.AddModelErrors(result);

                return View(formModel);
            }

            TempData.AddSuccessMessage(string.Format(UserRegisteredMessage, formModel.Username));

            return RedirectToAction(nameof(Index));
        }

        #endregion RegisterUser HttpPost

        #region Private Methods

        private void AddModelErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private async Task<ChangeUserRolesFormModel> GetChangeUserRolesFormModel(User user, int page, string search)
        {
            IList<string> userRoles = await this.userManager.GetRolesAsync(user);
            IList<string> availableRoles = await this.roleManager.Roles
                .Where(r => !userRoles.Contains(r.Name))
                .Select(r => r.Name)
                .ToListAsync();

            return new ChangeUserRolesFormModel()
            {
                CurrentPage = page,
                Search = search,
                Username = user.UserName,
                AvailableRolesListItem = availableRoles.Select(roleName => new SelectListItem()
                {
                    Text = roleName,
                    Value = roleName
                })
                .ToList()
                .OrderBy(r => r.Text),
                SelectedRolesListItem = userRoles.Select(roleName => new SelectListItem()
                {
                    Text = roleName,
                    Value = roleName
                })
                .ToList()
                .OrderBy(r => r.Text),
                UserRoles = userRoles.OrderBy(r => r)
            };
        }

        #endregion Private Methods
    }
}
