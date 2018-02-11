// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Database;
    using Indspire.Soaring.Engagement.Extensions;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.Models.RedemptionViewModels;
    using Indspire.Soaring.Engagement.Utils;
    using Indspire.Soaring.Engagement.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = RoleNames.Administrator)]
    public class RedemptionController : BaseController
    {
        public RedemptionController(
            ApplicationDbContext context,
            IInstanceSelector instanceSelector)
        {
            this.DatabaseContext = context ??
                throw new ArgumentNullException(nameof(context));

            this.InstanceSelector = instanceSelector ??
                throw new ArgumentNullException(nameof(instanceSelector));
        }

        public IActionResult Index(
            int page = 1,
            int pageSize = 20,
            string search = null)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            IEnumerable<Redemption> redemptions = null;

            var totalCount = 0;

            var selectedInstanceID = this.InstanceSelector.InstanceID;

            var filterFunc = string.IsNullOrWhiteSpace(search)
                ? new Func<Redemption, bool>(i => i.InstanceID == selectedInstanceID)
                : new Func<Redemption, bool>(i =>
                    i.InstanceID == selectedInstanceID &&
                    ((!string.IsNullOrWhiteSpace(i.Name) && i.Name.Contains(search)) ||
                     (!string.IsNullOrWhiteSpace(i.RedemptionNumber) && i.RedemptionNumber.Contains(search)) ||
                     (!string.IsNullOrWhiteSpace(i.Description) && i.Description.Contains(search))));

            redemptions = this.DatabaseContext.Redemption
                .Where(filterFunc)
                .OrderByDescending(i => i.CreatedDate)
                .Skip(skip)
                .Take(take);

            totalCount = this.DatabaseContext.Redemption
                .Where(filterFunc)
                .Count();

            return this.View(redemptions.ToPagedList(totalCount, page, pageSize, search));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var redemption = await this.DatabaseContext.Redemption
                .FirstOrDefaultAsync(m => m.RedemptionID == id);

            if (redemption == null)
            {
                return this.NotFound();
            }

            return this.View(redemption);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateRedemptionViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateRedemptionViewModel redemptionViewModel)
        {
            if (this.ModelState.IsValid)
            {
                var selectedInstanceID = this.InstanceSelector.InstanceID;

                var redemption = new Redemption
                {
                    InstanceID = selectedInstanceID,
                    RedemptionNumber = DataUtils.GenerateNumber(),
                    CreatedDate = DateTime.UtcNow,
                    PointsRequired = redemptionViewModel.PointsRequired
                };

                redemption.ModifiedDate = redemption.CreatedDate;

                if (redemptionViewModel != null)
                {
                    redemption.Name = redemptionViewModel.Name;
                    redemption.PointsRequired = redemptionViewModel.PointsRequired;
                    redemption.Description = redemptionViewModel.Description;
                }

                this.DatabaseContext.Add(redemption);

                await this.DatabaseContext.SaveChangesAsync();

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(redemptionViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var redemption = await this.DatabaseContext.Redemption
                .FirstOrDefaultAsync(m => m.RedemptionID == id);

            if (redemption == null)
            {
                return this.NotFound();
            }

            var viewModel = new EditRedemptionViewModel
            {
                RedemptionID = redemption.RedemptionID,
                Description = redemption.Description,
                Name = redemption.Name,
                PointsRequired = redemption.PointsRequired
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            EditRedemptionViewModel redemptionViewModel)
        {
            if (id != redemptionViewModel.RedemptionID)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    var redemption = await this.DatabaseContext.Redemption
                        .FirstOrDefaultAsync(i => i.RedemptionID == redemptionViewModel.RedemptionID);

                    if (redemption != null)
                    {
                        redemption.Name = redemptionViewModel.Name;
                        redemption.Description = redemptionViewModel.Description;
                        redemption.ModifiedDate = DateTime.UtcNow;
                        redemption.PointsRequired = redemptionViewModel.PointsRequired;
                    }

                    this.DatabaseContext.Update(redemption);

                    await this.DatabaseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.RedemptionExists(redemptionViewModel.RedemptionID))
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

            return this.View(redemptionViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var redemption = await this.DatabaseContext.Redemption
                .FirstOrDefaultAsync(m => m.RedemptionID == id);

            if (redemption == null)
            {
                return this.NotFound();
            }

            return this.View(redemption);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var redemption = await this.DatabaseContext.Redemption.SingleOrDefaultAsync(m => m.RedemptionID == id);

            this.DatabaseContext.Redemption.Remove(redemption);
            await this.DatabaseContext.SaveChangesAsync();

            return this.RedirectToAction(nameof(this.Index));
        }

        [AllowAnonymous]
        public IActionResult Scan(string redemptionNumber)
        {
            var viewModel = new RedemptionScanViewModel
            {
                RedemptionNumber = redemptionNumber,
                HasRedemptionNumber = !string.IsNullOrWhiteSpace(redemptionNumber)
            };

            if (viewModel.HasRedemptionNumber)
            {
                var redemption = this.DatabaseContext.Redemption
                    .FirstOrDefault(i => i.RedemptionNumber == viewModel.RedemptionNumber);

                if (redemption == null)
                {
                    viewModel.RedemptionNumberInvalid = true;
                }
                else
                {
                    viewModel.Redemption = redemption;
                }
            }

            return this.View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogAction(
            string redemptionNumber,
            string userNumber)
        {
            var viewModel = new LogRedemptionJsonViewModel();

            try
            {
                // validate
                var redemption = await this.DatabaseContext.Redemption
                    .FirstOrDefaultAsync(i => i.RedemptionNumber == redemptionNumber);

                if (redemption == null)
                {
                    throw new ApplicationException("Redemption not found.");
                }

                var user = await this.DatabaseContext.Attendee
                    .FirstOrDefaultAsync(i => i.UserNumber == userNumber);

                if (user == null)
                {
                    throw new ApplicationException("User not found.");
                }

                var existingRedemptionByUser = await this.DatabaseContext.RedemptionLog
                    .FirstOrDefaultAsync(i =>
                        i.UserID == user.UserID &&
                        i.RedemptionID == redemption.RedemptionID);

                if (existingRedemptionByUser != null)
                {
                    throw new ApplicationException($"User has already Redeemed this. User has {PointsUtils.GetPointsForUser(user.UserID, this.DatabaseContext)} points.");
                }

                // check if we have enough points
                var pointsShort = 0;
                var userPoints = PointsUtils.GetPointsForUser(user.UserID, this.DatabaseContext);
                var pointsRequired = redemption.PointsRequired;
                var hasEnoughPoints = false;

                pointsShort = pointsRequired - userPoints;

                if (pointsShort <= 0)
                {
                    hasEnoughPoints = true;
                    pointsShort = 0;
                }

                if (hasEnoughPoints)
                {
                    // good to go!
                    var redemptionLog = new RedemptionLog
                    {
                        RedemptionID = redemption.RedemptionID,
                        CreatedDate = DateTime.UtcNow,
                        UserID = user.UserID
                    };
                    redemptionLog.ModifiedDate = redemptionLog.CreatedDate;

                    this.DatabaseContext.Add(redemptionLog);

                    await this.DatabaseContext.SaveChangesAsync();
                }

                viewModel.ResponseData.PointsShort = pointsShort;
                viewModel.ResponseData.Success = hasEnoughPoints;

                viewModel.ResponseData.PointsBalance =
                    PointsUtils.GetPointsForUser(user.UserID, this.DatabaseContext);

                viewModel.ResponseData.UserNumber = user.UserNumber;
                viewModel.ResponseData.ExternalID = user.ExternalID;
            }
            catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
                viewModel.ResponseData = null;
            }

            return new JsonResult(viewModel);
        }

        public async Task<IActionResult> List()
        {
            var selectedInstanceID = this.InstanceSelector.InstanceID;

            var topRedemptions = await this.DatabaseContext.RedemptionLog
                .Where(i => i.Redemption.InstanceID == selectedInstanceID)
                .GroupBy(i => i.RedemptionID)
                .Select(i => new RedemptionsRow
                {
                    RedemptionNumber = i.FirstOrDefault().Redemption.RedemptionNumber,
                    TimesRedeemed = i.Count(),
                    Name = i.FirstOrDefault().Redemption.Name,
                    Description = i.FirstOrDefault().Redemption.Description
                })
                .OrderByDescending(i => i.TimesRedeemed)
                .ToListAsync();

            return this.View(topRedemptions);
        }

        private bool RedemptionExists(int id)
        {
            return this.DatabaseContext.Redemption.Any(e => e.RedemptionID == id);
        }
    }
}
