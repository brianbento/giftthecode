// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Extensions;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    [Authorize]
    public class ApplicationUserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IEmailSender emailSender;

        private readonly ILogger logger;

        public ApplicationUserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 10)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            var users = await this.userManager.Users
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalCount = this.userManager.Users.Count();

            return this.View(users.ToPagedList(totalCount, page, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                nameof(ApplicationUser.Email),
                nameof(ApplicationUser.UserName))]
            ApplicationUser user,
            string password)
        {
            IActionResult actionResult = null;

            bool passwordMatches = password == confirmPassword;
            if (!passwordMatches)
            {
                ModelState.AddModelError(string.Empty, "Your Password and Confirm Password did not match");
            }

            if (ModelState.IsValid && passwordMatches)
            {
                var identityResult = await this.userManager.CreateAsync(user);

                if (identityResult.Succeeded)
                {
                    var passwordResult = await this.userManager.AddPasswordAsync(user, password);

                    if (passwordResult.Succeeded)
                    {

                        if (!await _userManager.IsInRoleAsync(user, RoleNames.Administrator))
                        {
                            await _userManager.AddToRoleAsync(user, RoleNames.Administrator);
                        }

                        actionResult = RedirectToAction(nameof(Index));
                    } else
                    {
                        await _userManager.DeleteAsync(user);
                        ModelState.AddModelError(string.Empty, "Password must be at least 6 characters, requires at least one non-alphanumeric character, at least one digit, at least one lowercase character, and at least one uppercase character.");
                        actionResult = View(user);
                    }
                } else
                {
                    ModelState.AddModelError(string.Empty, identityResult.ToString());
                    actionResult = View(user);
                }
                
            }
            else
            {
                // TODO: display specific error messages
                actionResult = this.View(user);
            }

            return actionResult;
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            IActionResult actionResult = null;

            var user = string.IsNullOrWhiteSpace(id)
                ? null
                : await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                actionResult = this.NotFound();
            }
            else
            {
                actionResult = this.View(user);
            }

            return actionResult;
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await this.userManager.DeleteAsync(user);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}