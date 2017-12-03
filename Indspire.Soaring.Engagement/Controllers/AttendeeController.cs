namespace Indspire.Soaring.Engagement.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Database;
    using Indspire.Soaring.Engagement.Extensions;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.Models.AttendeeViewModels;
    using Indspire.Soaring.Engagement.Utils;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Indspire.Soaring.Engagement.ViewModels;
    using System.Collections.Generic;

    [Authorize(Roles = RoleNames.Administrator)]
    public class AttendeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string search = null)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            List<Attendee> users = null;
            int totalCount = 0;

            if (string.IsNullOrEmpty(search))
            {
                users = await _context.Attendee
                    .OrderByDescending(i => i.CreatedDate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                totalCount = await _context.Attendee.CountAsync();
            } else
            {
                users = await _context.Attendee
                    .Where(i => i.UserNumber.Contains(search) || i.ExternalID.Contains(search))
                    .OrderByDescending(i => i.CreatedDate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                totalCount = await _context.Attendee
                    .Where(i => i.UserNumber.Contains(search) || i.ExternalID.Contains(search)).CountAsync();
            }

            return View(users.ToPagedList(totalCount, page, pageSize, search));
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var viewModel = new UserDetailsViewModel();

            if (id == null)
            {
                return NotFound();
            }

            var attendee = await _context.Attendee
                .SingleOrDefaultAsync(m => m.UserID == id);

            if (attendee == null)
            {
                return NotFound();
            }

            viewModel.User = attendee;
            viewModel.PointsBalance = PointsUtils.GetPointsForUser(attendee.UserID, _context);

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateAttendeeViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAttendeeViewModel attendeeViewModel)
        {
            var dataUtils = new DataUtils();

            if (ModelState.IsValid)
            {
                var attendee = new Attendee();

                attendee.ModifiedDate = attendee.CreatedDate = DateTime.UtcNow;
                attendee.Deleted = false;
                attendee.UserNumber = dataUtils.GenerateNumber();

                if (attendeeViewModel != null)
                {
                    attendee.ExternalID = attendeeViewModel.ExternalID;
                }

                _context.Add(attendee);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(attendeeViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Attendee.SingleOrDefaultAsync(m => m.UserID == id);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditAttendeeViewModel();

            viewModel.ExternalID = user.ExternalID;
            viewModel.UserID = user.UserID;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAttendeeViewModel attendeeViewModel)
        {
            if (id != attendeeViewModel.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var attendee = _context.Attendee
                        .FirstOrDefault(i => i.UserID == attendeeViewModel.UserID);

                    if (attendee != null)
                    {
                        attendee.ExternalID = attendeeViewModel.ExternalID;
                        attendee.ModifiedDate = DateTime.UtcNow;
                    }

                    _context.Update(attendee);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendeeExists(attendeeViewModel.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(attendeeViewModel);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendee = await _context.Attendee
                .SingleOrDefaultAsync(m => m.UserID == id);

            if (attendee == null)
            {
                return NotFound();
            }

            return View(attendee);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendee = await _context.Attendee.SingleOrDefaultAsync(m => m.UserID == id);

            _context.Attendee.Remove(attendee);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool AttendeeExists(int id)
        {
            return _context.Attendee.Any(e => e.UserID == id);
        }

        [AllowAnonymous]
        [Route("[controller]/scan")]
        public IActionResult Scan()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[controller]/checkbalance")]
        public async Task<IActionResult> CheckBalance(string UserNumber)
        {
            var viewModel = new CheckBalanceJsonViewModel();
            try
            {

                //validate 
                var user = await _context.Attendee.FirstOrDefaultAsync(i => i.UserNumber == UserNumber);

                if (user == null)
                {
                    throw new ApplicationException("User not found.");
                }


                viewModel.ResponseData.PointsBalance = PointsUtils.GetPointsForUser(user.UserID, _context);
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

        public async Task<IActionResult> BulkCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BulkCreateConfirmation(int amount)
        {
            var viewModel = new BulkCreateViewModel();
            viewModel.Amount = amount;
            int usersCreated = 0;
            int maxUsers = 2000;

            try
            {
                if (amount < 1)
                {
                    throw new ApplicationException("Amount to create must be greater than 0");
                }

                if (amount > maxUsers)
                {
                    throw new ApplicationException($"Amount to create must be less than {maxUsers}");
                }


                for(var i = 0; i < amount; i++)
                {
                    var dataUtils = new DataUtils();
                    Attendee user = new Attendee();
                    user.UserNumber = dataUtils.GenerateNumber();
                    user.CreatedDate = DateTime.UtcNow;
                    user.ModifiedDate = user.CreatedDate;   
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    usersCreated++;
                }

                viewModel.AmountCreated = usersCreated;
                viewModel.Success = true;

                return View("BulkCreateSuccess", viewModel);
            }catch (Exception ex)
            {
                viewModel.Success = false;
                viewModel.AmountCreated = usersCreated;
                viewModel.ErrorMessage = $"An error occurred while trying to Bulk Create Users. Error: {ex.Message}.";
                return View("BulkCreateFailed", viewModel);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SetExternalID(string userNumber, string externalID)
        {
            var viewModel = new SetUserExternalIDJsonViewModel();
            try
            {

                var user = await _context.Attendee.FirstOrDefaultAsync(i => i.UserNumber == userNumber);

                if(user == null)
                {
                    throw new ApplicationException("User not found");
                }

                user.ExternalID = externalID;

                _context.Update(user);

                await _context.SaveChangesAsync();

                viewModel.ResponseData.Success = true;

            } catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
                viewModel.ResponseData.Success = false;
            }

            return Json(viewModel);
        }

    }
}
