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
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = RoleNames.Administrator)]
    public class InstanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            var instance = await _context.Instance
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalCount = await _context.Instance.CountAsync();

            return View(instance.ToPagedList(totalCount, page, pageSize));
        }

        public IActionResult Create()
        {
            var viewModel = new CreateInstanceViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInstanceViewModel instanceViewModel)
        {
            var dataUtils = new DataUtils();

            if (ModelState.IsValid)
            {
                var instance = new Instance();

                if (instanceViewModel != null)
                {
                    instance.ModifiedDate = instance.CreatedDate = DateTime.UtcNow;
                    instance.Description = instanceViewModel.Description;
                    instance.Name = instanceViewModel.Name;
                }

                _context.Add(instance);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(instanceViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instance = await _context.Instance
                .SingleOrDefaultAsync(m => m.InstanceID == id);

            if (instance == null)
            {
                return NotFound();
            }

            return View(instance);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instance = await _context.Instance
                .FirstOrDefaultAsync(m => m.InstanceID == id);

            if (instance != null)
            {
                _context.Instance.Remove(instance);

                await _context.SaveChangesAsync();

                // TODO: move this code to its own class
                var selectedInstanceCookieIDAsString = HttpContext.Request.Cookies["InstanceID"];

                var selectedInstanceID = 0;

                int.TryParse(selectedInstanceCookieIDAsString, out selectedInstanceID);

                // if the deleted instance was the selected one,
                // defaults to the first instance, if any.
                if (instance.InstanceID == selectedInstanceID)
                {
                    var firstInstance = await _context.Instance
                        .FirstOrDefaultAsync(m => m.InstanceID == id);

                    if (firstInstance != null)
                    {
                        HttpContext.Response.Cookies.Append(
                            "InstanceID",
                            firstInstance.InstanceID.ToString(),
                            new CookieOptions()
                            {
                                Secure = true
                            });
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Select(SelectInstanceViewModel instanceViewModel)
        {
            var returnUrl = "/admin";

            if (ModelState.IsValid)
            {
                try
                {
                    var instance = _context.Instance
                        .FirstOrDefault(i => i.InstanceID == instanceViewModel.InstanceID);

                    if (instance != null)
                    {
                        HttpContext.Response.Cookies.Append(
                            "InstanceID",
                            instance.InstanceID.ToString(),
                            new CookieOptions()
                            {
                                Secure = true
                            });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstanceExists(instanceViewModel.InstanceID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Instance.SingleOrDefaultAsync(m => m.InstanceID == id);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditInstanceViewModel();

            viewModel.DefaultInstance = user.DefaultInstance;
            viewModel.Description = user.Description;
            viewModel.Name = user.Name;
            viewModel.InstanceID = user.InstanceID;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditInstanceViewModel instanceViewModel)
        {
            if (id != instanceViewModel.InstanceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var instance = _context.Instance
                        .FirstOrDefault(i => i.InstanceID == instanceViewModel.InstanceID);

                    if (instance != null)
                    {
                        instance.Name = instanceViewModel.Name;
                        instance.DefaultInstance = instanceViewModel.DefaultInstance;
                        instance.Description = instanceViewModel.Description;
                        instance.ModifiedDate = DateTime.UtcNow;
                    }

                    _context.Update(instance);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstanceExists(instanceViewModel.InstanceID))
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

            return View(instanceViewModel);
        }

        private bool InstanceExists(int id)
        {
            return _context.Instance.Any(e => e.InstanceID == id);
        }
    }
}