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
    public class AtendeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AtendeeController(ApplicationDbContext context)
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

            var atendee = await _context.User
                .SingleOrDefaultAsync(m => m.UserID == id);

            if (atendee == null)
            {
                return NotFound();
            }

            return View(atendee);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateAtendeeViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAtendeeViewModel atendeeViewModel)
        {
            var dataUtils = new DataUtils();

            if (ModelState.IsValid)
            {
                var atendee = new User();

                atendee.ModifiedDate = atendee.CreatedDate = DateTime.UtcNow;
                atendee.Deleted = false;
                atendee.UserNumber = dataUtils.GenerateNumber();

                if (atendeeViewModel != null)
                {
                    atendee.ExternalID = atendeeViewModel.ExternalID;
                }

                _context.Add(atendee);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(atendeeViewModel);
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
        public async Task<IActionResult> Edit(int id, EditAtendeeViewModel atendeeViewModel)
        {
            if (id != atendeeViewModel.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var atendee = _context.User
                        .FirstOrDefault(i => i.UserID == atendeeViewModel.UserID);

                    if (atendee != null)
                    {
                        atendee.ExternalID = atendeeViewModel.ExternalID;
                        atendee.ModifiedDate = DateTime.UtcNow;
                    }

                    _context.Update(atendee);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(atendeeViewModel.UserID))
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

            return View(atendeeViewModel);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atendee = await _context.User
                .SingleOrDefaultAsync(m => m.UserID == id);

            if (atendee == null)
            {
                return NotFound();
            }

            return View(atendee);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var atendee = await _context.User.SingleOrDefaultAsync(m => m.UserID == id);

            _context.User.Remove(atendee);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserID == id);
        }        
    }
}
