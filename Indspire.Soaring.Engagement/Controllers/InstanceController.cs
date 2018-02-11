// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Database;
    using Indspire.Soaring.Engagement.Extensions;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.Models.InstanceViewModels;
    using Indspire.Soaring.Engagement.Utils;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = RoleNames.Administrator)]
    public class InstanceController : BaseController
    {
        public InstanceController(
            ApplicationDbContext context,
            IInstanceSelector instanceSelector)
        {
            this.DatabaseContext = context ??
                throw new ArgumentNullException(nameof(context));

            this.InstanceSelector = instanceSelector ??
                throw new ArgumentNullException(nameof(instanceSelector));
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            var instance = await this.DatabaseContext.Instance
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalCount = await this.DatabaseContext.Instance.CountAsync();

            return this.View(instance.ToPagedList(totalCount, page, pageSize));
        }

        public IActionResult Create()
        {
            var viewModel = new CreateInstanceViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInstanceViewModel instanceViewModel)
        {
            if (this.ModelState.IsValid)
            {
                var instance = new Instance();

                if (instanceViewModel != null)
                {
                    instance.ModifiedDate = instance.CreatedDate = DateTime.UtcNow;
                    instance.Description = instanceViewModel.Description;
                    instance.Name = instanceViewModel.Name;
                }

                this.DatabaseContext.Add(instance);

                await this.DatabaseContext.SaveChangesAsync();

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(instanceViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var instance = await this.DatabaseContext.Instance
                .FirstOrDefaultAsync(m => m.InstanceID == id);

            if (instance == null)
            {
                return this.NotFound();
            }

            return this.View(instance);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instance = await this.DatabaseContext.Instance
                .FirstOrDefaultAsync(m => m.InstanceID == id);

            if (instance != null)
            {
                this.DatabaseContext.Instance.Remove(instance);

                await this.DatabaseContext.SaveChangesAsync();

                var selectedInstanceID = this.InstanceSelector.InstanceID;

                // if the deleted instance was the selected one,
                // defaults to the first instance, if any.
                if (instance.InstanceID == selectedInstanceID)
                {
                    var firstInstance = await this.DatabaseContext.Instance
                        .FirstOrDefaultAsync(m => m.InstanceID == id);

                    if (firstInstance != null)
                    {
                        this.InstanceSelector.InstanceID = firstInstance.InstanceID;
                    }
                }
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Select(SelectInstanceViewModel instanceViewModel)
        {
            var returnUrl = "/admin";

            if (this.ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(instanceViewModel.ReturnUrl))
                {
                    returnUrl = instanceViewModel.ReturnUrl;
                }

                try
                {
                    var instance = await this.DatabaseContext.Instance
                        .FirstOrDefaultAsync(i => i.InstanceID == instanceViewModel.InstanceID);

                    if (instance != null)
                    {
                        this.InstanceSelector.InstanceID = instance.InstanceID;
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.InstanceExists(instanceViewModel.InstanceID))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return this.Redirect(returnUrl);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this.DatabaseContext.Instance.SingleOrDefaultAsync(m => m.InstanceID == id);

            if (user == null)
            {
                return this.NotFound();
            }

            var viewModel = new EditInstanceViewModel
            {
                DefaultInstance = user.DefaultInstance,
                Description = user.Description,
                Name = user.Name,
                InstanceID = user.InstanceID
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditInstanceViewModel instanceViewModel)
        {
            if (id != instanceViewModel.InstanceID)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    var instance = this.DatabaseContext.Instance
                        .FirstOrDefault(i => i.InstanceID == instanceViewModel.InstanceID);

                    if (instance != null)
                    {
                        instance.Name = instanceViewModel.Name;
                        instance.DefaultInstance = instanceViewModel.DefaultInstance;
                        instance.Description = instanceViewModel.Description;
                        instance.ModifiedDate = DateTime.UtcNow;
                    }

                    this.DatabaseContext.Update(instance);

                    await this.DatabaseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.InstanceExists(instanceViewModel.InstanceID))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(instanceViewModel);
        }

        private bool InstanceExists(int id)
        {
            return this.DatabaseContext.Instance.Any(e => e.InstanceID == id);
        }
    }
}