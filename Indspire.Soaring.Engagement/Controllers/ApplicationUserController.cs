// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Extensions;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.Models.AccountViewModels;
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
            var viewModel = new RegisterViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel registerViewModel)
        {
            IActionResult actionResult = null;

            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email
                };

                var existentUser = await this.userManager
                    .FindByNameAsync(registerViewModel.UserName);

                if (existentUser == null)
                {
                    var identityResult = await this.userManager.CreateAsync(
                        user,
                        registerViewModel.Password);

                    if (identityResult.Succeeded)
                    {
                        actionResult = this.RedirectToAction(nameof(this.Index));
                    }
                    else
                    {
                        const string message =
                            "Password must be at least 6 characters, " +
                            "requires at least one non-alphanumeric character," +
                            "at least one digit, " +
                            "at least one lowercase character," +
                            "and at least one uppercase character.";

                        this.ModelState.AddModelError(
                            nameof(registerViewModel.Password),
                            message);

                        registerViewModel.ClearPassword();

                        actionResult = this.View(registerViewModel);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(
                        nameof(registerViewModel.UserName),
                        "The user name already exists");

                    registerViewModel.ClearPassword();

                    actionResult = this.View(registerViewModel);
                }
            }
            else
            {
                registerViewModel.ClearPassword();

                actionResult = this.View(registerViewModel);
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