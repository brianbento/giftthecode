// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class AdminController : BaseController
    {
        private int maxNumberOfResults = 25;

        public AdminController(
            ApplicationDbContext context,
            IInstanceSelector instanceSelector)
        {
            this.DatabaseContext = context ??
                throw new ArgumentNullException(nameof(context));

            this.InstanceSelector = instanceSelector ??
                throw new ArgumentNullException(nameof(instanceSelector));
        }

        [HttpGet]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Index()
        {
            var selectedInstanceID = this.InstanceSelector.InstanceID;

            var topAwarded = await this.DatabaseContext.AwardLog
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
                  .Take(this.maxNumberOfResults)
                  .ToListAsync();

            var topUsers = await this.DatabaseContext.AwardLog
                .Where(i => i.Award.InstanceID == selectedInstanceID)
                .GroupBy(i => i.UserID)
                .Select(i => new AttendeeRow
                {
                    UserNamber = i.FirstOrDefault().User.UserNumber,
                    ExternalId = i.FirstOrDefault().User.ExternalID,
                    Points = i.Sum(p => p.Points)
                })
                .OrderByDescending(i => i.Points)
                .Take(this.maxNumberOfResults)
                .ToListAsync();

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
                .Take(this.maxNumberOfResults)
                .ToListAsync();

            var dashboardReports = new DashboardReports
            {
                AwardsList = topAwarded,
                AttendeeList = topUsers,
                RedemptionsList = topRedemptions
            };

            return this.View("~/Views/Admin/Admin.cshtml", dashboardReports);
        }
    }
}