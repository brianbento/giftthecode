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
    using Indspire.Soaring.Engagement.Models.AwardViewModels;
    using Indspire.Soaring.Engagement.Utils;
    using Indspire.Soaring.Engagement.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = RoleNames.Administrator)]
    public class AwardController : BaseController
    {
        public AwardController(
            ApplicationDbContext context,
            IInstanceSelector instanceSelector)
        {
            this.DatabaseContext = context ??
                throw new ArgumentNullException(nameof(context));

            this.InstanceSelector = instanceSelector ??
                throw new ArgumentNullException(nameof(instanceSelector));
        }

        public IActionResult Index(int page = 1, int pageSize = 10, string search = null)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            IEnumerable<Award> awards = new List<Award>();
            var totalCount = 0;

            var selectedInstanceID = this.InstanceSelector.InstanceID;

            var filterFunc = string.IsNullOrWhiteSpace(search)
                ? new Func<Award, bool>(i => i.InstanceID == selectedInstanceID)
                : new Func<Award, bool>(i =>
                    i.InstanceID == selectedInstanceID &&
                    ((!string.IsNullOrWhiteSpace(i.AwardNumber) && i.AwardNumber.Contains(search)) ||
                     (!string.IsNullOrWhiteSpace(i.Name) && i.Name.Contains(search)) ||
                     (!string.IsNullOrWhiteSpace(i.Description) && i.Description.Contains(search))));

            awards = this.DatabaseContext.Award
                .Where(filterFunc)
                .OrderByDescending(i => i.CreatedDate)
                .Skip(skip)
                .Take(take);

            totalCount = this.DatabaseContext.Award
                .Where(filterFunc)
                .Count();

            return this.View(awards.ToPagedList(totalCount, page, pageSize, search));
        }

        public async Task<IActionResult> List()
        {
            var selectedInstanceID = this.InstanceSelector.InstanceID;

            var lst = await this.DatabaseContext.AwardLog
                .Where(i => i.Award.InstanceID == selectedInstanceID)
                .GroupBy(i => i.AwardID)
                .Select(i => new AwardsRow
                {
                    AwardNumber = i.FirstOrDefault().Award.AwardNumber,
                    TimesAwards = i.Count(),
                    Name = i.FirstOrDefault().Award.Name,
                    Description = i.FirstOrDefault().Award.Description
                })
                .OrderByDescending(i => i.TimesAwards)
                .ToListAsync();

            return this.View(lst);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[controller]/[action]/{awardnumber}")]
        public async Task<IActionResult> Scan(string awardNumber)
        {
            var viewModel = new AwardScanViewModel();

            viewModel.AwardNumber = awardNumber;

            viewModel.HasAwardNumber = !string.IsNullOrWhiteSpace(viewModel.AwardNumber);

            if (viewModel.HasAwardNumber)
            {
                var award = await this.DatabaseContext.Award
                    .FirstOrDefaultAsync(i => i.AwardNumber == viewModel.AwardNumber);

                if (award == null)
                {
                    viewModel.AwardNumberInvalid = true;
                }
                else
                {
                    viewModel.Award = award;
                }
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Scan")]
        [AllowAnonymous]
        public IActionResult PostScan(string awardNumber)
        {
            var result = string.IsNullOrWhiteSpace(awardNumber)
                ? this.RedirectToAction("Scan", new { AwardNumber = 0 })
                : this.RedirectToAction("Scan", new { AwardNumber = awardNumber });

            return result;
        }

        public async Task<IActionResult> Details(int? id)
        {
            IActionResult result = null;

            if (id == null)
            {
                result = this.NotFound();
            }
            else
            {
                var award = await this.DatabaseContext.Award
                    .SingleOrDefaultAsync(m => m.AwardID == id);

                if (award == null)
                {
                    result = this.NotFound();
                }
                else
                {
                    result = this.View(award);
                }
            }

            return result;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateAwardViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAwardViewModel awardViewModel)
        {
            var dataUtils = new DataUtils();

            if (this.ModelState.IsValid)
            {
                var award = new Award
                {
                    InstanceID = this.InstanceSelector.InstanceID,
                    Name = awardViewModel.Name,
                    Description = awardViewModel.Description,
                    Points = awardViewModel.Points,
                    AwardNumber = dataUtils.GenerateNumber(),
                    CreatedDate = DateTime.UtcNow
                };

                award.ModifiedDate = award.CreatedDate;

                this.DatabaseContext.Add(award);

                await this.DatabaseContext.SaveChangesAsync();

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(awardViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            IActionResult actionResult = null;

            var award = id.HasValue
              ? await this.DatabaseContext.Award.FirstOrDefaultAsync(m => m.AwardID == id)
              : null;

            if (award == null)
            {
                actionResult = this.NotFound();
            }
            else
            {
                var viewModel = new EditAwardViewModel
                {
                    AwardID = award.AwardID,
                    Description = award.Description,
                    Name = award.Name,
                    Points = award.Points
                };

                actionResult = this.View(viewModel);
            }

            return actionResult;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAwardViewModel awardViewModel)
        {
            if (id != awardViewModel.AwardID)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    var award = await this.DatabaseContext.Award
                        .FirstOrDefaultAsync(i => i.AwardID == awardViewModel.AwardID);

                    if (award != null)
                    {
                        award.Name = awardViewModel.Name;
                        award.Description = awardViewModel.Description;
                        award.Points = awardViewModel.Points;
                        award.ModifiedDate = DateTime.UtcNow;
                    }

                    this.DatabaseContext.Update(award);

                    await this.DatabaseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.AwardExists(awardViewModel.AwardID))
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

            return this.View(awardViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            IActionResult actionResult = null;

            var award = id.HasValue
              ? await this.DatabaseContext.Award.FirstOrDefaultAsync(m => m.AwardID == id)
              : null;

            if (award == null)
            {
                actionResult = this.NotFound();
            }
            else
            {
                actionResult = this.View(award);
            }

            return actionResult;
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var award = await this.DatabaseContext.Award.FirstOrDefaultAsync(m => m.AwardID == id);

            if (award != null)
            {
                this.DatabaseContext.Award.Remove(award);
                await this.DatabaseContext.SaveChangesAsync();
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        private bool AwardExists(int id)
        {
            return this.DatabaseContext.Award.Any(e => e.AwardID == id);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogAction(
            string AwardNumber,
            string UserNumber)
        {
            var viewModel = new LogActionJsonViewModel();

            try
            {
                // validate 
                var award = await this.DatabaseContext.Award
                    .FirstOrDefaultAsync(i => i.AwardNumber == AwardNumber);

                if (award == null)
                {
                    throw new ApplicationException("Award not found.");
                }

                var user = await this.DatabaseContext.Attendee
                    .FirstOrDefaultAsync(i => i.UserNumber == UserNumber);

                if (user == null)
                {
                    throw new ApplicationException("User not found.");
                }

                var existingAwardLogByThisUser = await this.DatabaseContext.AwardLog
                    .FirstOrDefaultAsync(i =>
                        i.UserID == user.UserID &&
                        i.AwardID == award.AwardID);

                if (existingAwardLogByThisUser != null)
                {
                    throw new ApplicationException($"User has already been Awarded points for this action. User has {PointsUtils.GetPointsForUser(user.UserID, this.DatabaseContext)} points.");
                }

                // good to go!

                var awardLog = new AwardLog();
                awardLog.AwardID = award.AwardID;
                awardLog.CreatedDate = DateTime.UtcNow;
                awardLog.ModifiedDate = awardLog.CreatedDate;
                awardLog.Points = award.Points;
                awardLog.UserID = user.UserID;

                this.DatabaseContext.Add(awardLog);

                await this.DatabaseContext.SaveChangesAsync();

                viewModel.ResponseData.PointsAwarded = awardLog.Points;
                viewModel.ResponseData.PointsBalance = PointsUtils.GetPointsForUser(user.UserID, this.DatabaseContext);
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
    }
}