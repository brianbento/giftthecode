namespace Indspire.Soaring.Engagement.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Database;
    using Indspire.Soaring.Engagement.Extensions;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.Utils;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Indspire.Soaring.Engagement.ViewModels;

    [Authorize(Roles = RoleNames.Administrator)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            var users = await _context.User
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalCount = await _context.User.CountAsync();

            return View(users.ToPagedList(totalCount, page, pageSize));
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var viewModel = new UserDetailsViewModel();

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            viewModel.User = user;
            viewModel.PointsBalance = PointsUtils.GetPointsForUser(user.UserID, _context);

            return View(viewModel);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExternalID")] User user)
        {
            var dataUtils = new DataUtils();

            if (ModelState.IsValid)
            {
                user.CreatedDate = DateTime.UtcNow;
                user.ModifiedDate = user.CreatedDate;
                user.Deleted = false;
                user.UserNumber = dataUtils.GenerateNumber();

                _context.Add(user);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,ExternalID")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userFromDatabase = _context.User
                        .FirstOrDefault(i => i.UserID == user.UserID);

                    if (userFromDatabase != null)
                    {
                        userFromDatabase.ExternalID = user.ExternalID;
                        userFromDatabase.ModifiedDate = DateTime.UtcNow;
                    }

                    _context.Update(userFromDatabase);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.SingleOrDefaultAsync(m => m.UserID == id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserID == id);
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
                var user = await _context.User.FirstOrDefaultAsync(i => i.UserNumber == UserNumber);

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
                    User user = new User();
                    user.UserNumber = dataUtils.GenerateNumber();
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

                var user = await _context.User.FirstOrDefaultAsync(i => i.UserNumber == userNumber);

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
