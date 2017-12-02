namespace Indspire.Soaring.Engagement.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Database;
    using Indspire.Soaring.Engagement.Extensions;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.Models.AtendeeViewModels;
    using Indspire.Soaring.Engagement.Utils;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

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

        public IActionResult Create()
        {
            var viewModel = new CreateAtendeeViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAtendeeViewModel userViewModel)
        {
            var dataUtils = new DataUtils();

            if (ModelState.IsValid)
            {
                var user = new User();

                user.ModifiedDate = user.CreatedDate = DateTime.UtcNow;
                user.Deleted = false;
                user.UserNumber = dataUtils.GenerateNumber();

                if (userViewModel != null)
                {
                    user.ExternalID = userViewModel.ExternalID;
                }

                _context.Add(user);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(userViewModel);
        }

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

            var viewModel = new EditAtendeeViewModel();

            viewModel.ExternalID = user.ExternalID;
            viewModel.UserID = user.UserID;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAtendeeViewModel userViewModel)
        {
            if (id != userViewModel.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userFromDatabase = _context.User
                        .FirstOrDefault(i => i.UserID == userViewModel.UserID);

                    if (userFromDatabase != null)
                    {
                        userFromDatabase.ExternalID = userViewModel.ExternalID;
                        userFromDatabase.ModifiedDate = DateTime.UtcNow;
                    }

                    _context.Update(userFromDatabase);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userViewModel.UserID))
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

            return View(userViewModel);
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

        
    }
}
