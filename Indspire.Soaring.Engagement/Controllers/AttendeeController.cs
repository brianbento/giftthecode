﻿// Copyright (c) Team Agility. All rights reserved.

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
    using Indspire.Soaring.Engagement.Models.AttendeeViewModels;
    using Indspire.Soaring.Engagement.Utils;
    using Indspire.Soaring.Engagement.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = RoleNames.Administrator)]
    public class AttendeeController : BaseController
    {
        public AttendeeController(
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
            int pageSize = 10,
            string search = null)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            IEnumerable<Attendee> users = new List<Attendee>();

            var totalCount = 0;

            var selectedInstanceID = this.InstanceSelector.InstanceID;

            var filterFunc = string.IsNullOrWhiteSpace(search)
                ? new Func<Attendee, bool>(i => i.InstanceID == selectedInstanceID)
                : new Func<Attendee, bool>(i =>
                    i.InstanceID == selectedInstanceID &&
                    ((!string.IsNullOrWhiteSpace(i.UserNumber) && i.UserNumber.Contains(search)) ||
                     (!string.IsNullOrWhiteSpace(i.ExternalID) && i.ExternalID.Contains(search))));

            users = this.DatabaseContext.Attendee
                .Where(filterFunc)
                .OrderByDescending(i => i.CreatedDate)
                .Skip(skip)
                .Take(take);

            totalCount = this.DatabaseContext.Attendee
                .Where(filterFunc)
                .Count();

            return this.View(users.ToPagedList(totalCount, page, pageSize, search));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            IActionResult actionResult = null;

            var viewModel = new UserDetailsViewModel();

            var attendee = id.HasValue
                ? await this.DatabaseContext.Attendee
                    .FirstOrDefaultAsync(m => m.UserID == id)
                : null;

            if (attendee == null)
            {
                actionResult = this.NotFound();
            }
            else
            {
                viewModel.User = attendee;

                viewModel.PointsBalance = PointsUtils.GetPointsForUser(
                    attendee.UserID,
                    this.DatabaseContext);

                actionResult = this.View(viewModel);
            }

            return actionResult;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateAttendeeViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateAttendeeViewModel attendeeViewModel)
        {
            IActionResult actionResult = null;

            if (this.ModelState.IsValid)
            {
                var selectedInstanceID = this.InstanceSelector.InstanceID;

                var attendee = new Attendee();

                attendee.InstanceID = selectedInstanceID;
                attendee.ModifiedDate = attendee.CreatedDate = DateTime.UtcNow;
                attendee.Deleted = false;
                attendee.UserNumber = DataUtils.GenerateNumber();
                attendee.ExternalID = attendeeViewModel == null
                    ? string.Empty
                    : attendeeViewModel.ExternalID;

                this.DatabaseContext.Add(attendee);

                await this.DatabaseContext.SaveChangesAsync();

                actionResult = this.RedirectToAction(nameof(this.Index));
            }
            else
            {
                actionResult = this.View(attendeeViewModel);
            }

            return actionResult;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            IActionResult actionResult = null;

            var user = id.HasValue
                ? await this.DatabaseContext.Attendee.FirstOrDefaultAsync(m => m.UserID == id)
                : null;

            if (user == null)
            {
                actionResult = this.NotFound();
            }
            else
            {
                var viewModel = new EditAttendeeViewModel
                {
                    ExternalID = user.ExternalID,
                    UserID = user.UserID
                };

                actionResult = this.View(viewModel);
            }

            return actionResult;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            EditAttendeeViewModel attendeeViewModel)
        {
            if (id != attendeeViewModel.UserID)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    var attendee = this.DatabaseContext.Attendee
                        .FirstOrDefault(i => i.UserID == attendeeViewModel.UserID);

                    if (attendee != null)
                    {
                        attendee.ExternalID = attendeeViewModel.ExternalID;
                        attendee.ModifiedDate = DateTime.UtcNow;
                    }

                    this.DatabaseContext.Update(attendee);

                    await this.DatabaseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.AttendeeExists(attendeeViewModel.UserID))
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

            return this.View(attendeeViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            IActionResult actionResult = null;

            var attendee = id.HasValue
                ? await this.DatabaseContext.Attendee
                    .FirstOrDefaultAsync(m => m.UserID == id)
                : null;

            if (attendee == null)
            {
                actionResult = this.NotFound();
            }
            else
            {
                actionResult = this.View(attendee);
            }

            return actionResult;
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendee = await this.DatabaseContext.Attendee
                .FirstOrDefaultAsync(m => m.UserID == id);

            IActionResult actionResult = null;

            if (attendee == null)
            {
                actionResult = this.NotFound();
            }
            else
            {
                this.DatabaseContext.Attendee.Remove(attendee);

                await this.DatabaseContext.SaveChangesAsync();

                actionResult = this.RedirectToAction(nameof(this.Index));
            }

            return actionResult;
        }

        public async Task<IActionResult> List()
        {
            var selectedInstanceID = this.InstanceSelector.InstanceID;

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
                .ToListAsync();

            return this.View(topUsers);
        }

        [AllowAnonymous]
        [Route("[controller]/scan")]
        public IActionResult Scan()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[controller]/checkbalance")]
        public async Task<IActionResult> CheckBalance(string userNumber)
        {
            var viewModel = new CheckBalanceJsonViewModel();
            try
            {
                // validate
                var user = await this.DatabaseContext.Attendee
                    .FirstOrDefaultAsync(i => i.UserNumber == userNumber);

                if (user == null)
                {
                    throw new ApplicationException("User not found.");
                }

                viewModel.ResponseData.PointsBalance = PointsUtils
                    .GetPointsForUser(user.UserID, this.DatabaseContext);

                viewModel.ResponseData.UserNumber = user.UserNumber;
                viewModel.ResponseData.ExternalID = user.ExternalID;
            }
            catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
                viewModel.ResponseData = null;
            }

            return this.Json(viewModel);
        }

        public IActionResult BulkCreate()
        {
            var viewModel = new BulkCreateViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkCreate(
            BulkCreateViewModel bulkCreateViewModel)
        {
            IActionResult actionResult = null;

            if (this.ModelState.IsValid)
            {
                var usersCreated = 0;

                var selectedInstanceID = this.InstanceSelector.InstanceID;

                try
                {
                    for (var i = 0; i < bulkCreateViewModel.Amount; i++)
                    {
                        var user = new Attendee
                        {
                            InstanceID = selectedInstanceID,
                            UserNumber = DataUtils.GenerateNumber(),
                            CreatedDate = DateTime.UtcNow
                        };

                        user.ModifiedDate = user.CreatedDate;

                        this.DatabaseContext.Add(user);

                        await this.DatabaseContext.SaveChangesAsync();

                        usersCreated++;
                    }

                    bulkCreateViewModel.AmountCreated = usersCreated;

                    actionResult = this.RedirectToAction(
                        nameof(this.BulkCreateSuccess),
                        bulkCreateViewModel);
                }
                catch (Exception)
                {
                    bulkCreateViewModel.AmountCreated = usersCreated;

                    actionResult = this.RedirectToAction(
                        nameof(this.BulkCreateFailed),
                        bulkCreateViewModel);
                }
            }
            else
            {
                actionResult = this.View(bulkCreateViewModel);
            }

            return actionResult;
        }

        [HttpGet]
        public IActionResult BulkCreateFailed(
            BulkCreateViewModel bulkCreateViewModel)
        {
            return this.View(bulkCreateViewModel);
        }

        [HttpGet]
        public IActionResult BulkCreateSuccess(
            BulkCreateViewModel bulkCreateViewModel)
        {
            return this.View(bulkCreateViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SetExternalID(
            string userNumber,
            string externalID)
        {
            var viewModel = new SetUserExternalIDJsonViewModel();

            try
            {
                var user = await this.DatabaseContext.Attendee.FirstOrDefaultAsync(
                    i => i.UserNumber == userNumber);

                if (user == null)
                {
                    throw new ApplicationException("User not found");
                }

                user.ExternalID = externalID;

                this.DatabaseContext.Update(user);

                await this.DatabaseContext.SaveChangesAsync();

                viewModel.ResponseData.Success = true;
            }
            catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
                viewModel.ResponseData.Success = false;
            }

            return this.Json(viewModel);
        }

        public IActionResult PrintQRCode(string userNumber)
        {
            IList<AttendeeLabel> labels = new List<AttendeeLabel>();

            // get the user
            var attendee = this.DatabaseContext.Attendee.FirstOrDefault(
                i => i.UserNumber == userNumber);

            if (attendee == null)
            {
                throw new ApplicationException("Attendee not found.");
            }
            else
            {
                labels.Add(new AttendeeLabel(attendee));
            }

            var memoryStream = QRCodeUtils.GenerateLabelsAsPDF(labels.ToList());

            return this.File(
                memoryStream,
                "application/pdf",
                $"attendee_{userNumber}_qr_code_use_avery-22806-labels.pdf");
        }

        public IActionResult Print()
        {
            return this.View();
        }

        public IActionResult PrintAllQRCodes()
        {
            // assume we are printing ALL attendees
            var labels = this.DatabaseContext.Attendee
                .Select(i => new AttendeeLabel(i))
                .ToList();

            var memoryStream = QRCodeUtils.GenerateLabelsAsPDF(labels);

            return this.File(
                memoryStream,
                "application/pdf",
                "qr-codes_use_avery-22806-labels.pdf");
        }

        public IActionResult PrintTestQRCodes(int num = 12)
        {
            var testNumber = "123456";
            var labels = new List<AttendeeLabel>();

            for (var i = 0; i < num; i++)
            {
                labels.Add(new AttendeeLabel
                {
                    UserNumber = testNumber
                });
            }

            var memoryStream = QRCodeUtils.GenerateLabelsAsPDF(labels.ToList());

            return this.File(
                memoryStream,
                "application/pdf",
                "test_qr_codes.pdf");
        }

        private bool AttendeeExists(int id)
        {
            return this.DatabaseContext.Attendee.Any(e => e.UserID == id);
        }
    }
}
