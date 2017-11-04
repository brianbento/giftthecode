using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Indspire.Soaring.Engagement.Data;
using Indspire.Soaring.Engagement.Database;
using Microsoft.AspNetCore.Authorization;
using Indspire.Soaring.Engagement.Models;

namespace Indspire.Soaring.Engagement.Controllers
{
    [Authorize(Roles = RoleNames.Administrator)]
    public class RedemptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RedemptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Redemptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Redemption.ToListAsync());
        }

        // GET: Redemptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redemption = await _context.Redemption
                .SingleOrDefaultAsync(m => m.RedemptionID == id);
            if (redemption == null)
            {
                return NotFound();
            }

            return View(redemption);
        }

        // GET: Redemptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Redemptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("RedemptionNumber,Name,Description")]
            Redemption redemption)
        {
            if (ModelState.IsValid)
            {
                redemption.ModifiedDate = redemption.CreatedDate = DateTime.UtcNow;
                redemption.Deleted = false;

                _context.Add(redemption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(redemption);
        }

        // GET: Redemptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redemption = await _context.Redemption.SingleOrDefaultAsync(m => m.RedemptionID == id);
            if (redemption == null)
            {
                return NotFound();
            }
            return View(redemption);
        }

        // POST: Redemptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [
            Bind("RedemptionID,RedemptionNumber,Name,Description")]
            Redemption redemption)
        {
            if (id != redemption.RedemptionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var redemptionFromDatabase = await _context.Redemption
                        .FirstOrDefaultAsync(i => i.RedemptionID == redemption.RedemptionID);

                    if (redemptionFromDatabase != null)
                    {
                        redemptionFromDatabase.RedemptionNumber = redemption.RedemptionNumber;
                        redemptionFromDatabase.Name = redemption.Name;
                        redemptionFromDatabase.Description = redemption.Description;
                        redemptionFromDatabase.ModifiedDate = DateTime.UtcNow;
                    }

                    _context.Update(redemptionFromDatabase);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RedemptionExists(redemption.RedemptionID))
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
            return View(redemption);
        }

        // GET: Redemptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redemption = await _context.Redemption
                .SingleOrDefaultAsync(m => m.RedemptionID == id);

            if (redemption == null)
            {
                return NotFound();
            }

            return View(redemption);
        }

        // POST: Redemptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var redemption = await _context.Redemption.SingleOrDefaultAsync(m => m.RedemptionID == id);
            _context.Redemption.Remove(redemption);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RedemptionExists(int id)
        {
            return _context.Redemption.Any(e => e.RedemptionID == id);
        }

        [AllowAnonymous]
        public IActionResult Scan()
        {
            return View();
        }
    }
}
