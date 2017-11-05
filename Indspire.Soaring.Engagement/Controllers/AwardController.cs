﻿using System;
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
using Indspire.Soaring.Engagement.ViewModels;

namespace Indspire.Soaring.Engagement.Controllers
{
    [Authorize(Roles = RoleNames.Administrator)]
    public class AwardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AwardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Award
        public async Task<IActionResult> Index()
        {
            return View(await _context.Award.ToListAsync());
        }

        public async Task<IActionResult> List()
        {
            return View(await _context.Award.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Scan()
        {
            var viewModel = new AwardScanViewModel();
            viewModel.AwardNumber = $"{Request.Query["AwardNumber"]}";
            if(string.IsNullOrEmpty(viewModel.AwardNumber))
            {
                viewModel.HasAwardNumber = false;
            } else
            {
                viewModel.HasAwardNumber = true;
            }

            if(viewModel.HasAwardNumber)
            {
                var award = _context.Award.FirstOrDefault(i => i.EventNumber == viewModel.AwardNumber);

                if(award == null)
                {
                    viewModel.AwardNumberInvalid = true;
                } else
                {
                    viewModel.Award = award;
                }
            }



            return View(viewModel);
        }

        // GET: Award/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _context.Award
                .SingleOrDefaultAsync(m => m.AwardID == id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        // GET: Award/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Award/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AwardID,VendorID,EventNumber,Name,Description,Points,Deleted,CreatedDate,ModifiedDate")] Award award)
        {
            if (ModelState.IsValid)
            {
                _context.Add(award);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(award);
        }

        // GET: Award/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _context.Award.SingleOrDefaultAsync(m => m.AwardID == id);
            if (award == null)
            {
                return NotFound();
            }
            return View(award);
        }

        // POST: Award/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AwardID,VendorID,EventNumber,Name,Description,Points,Deleted,CreatedDate,ModifiedDate")] Award award)
        {
            if (id != award.AwardID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(award);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AwardExists(award.AwardID))
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
            return View(award);
        }

        // GET: Award/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _context.Award
                .SingleOrDefaultAsync(m => m.AwardID == id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        // POST: Award/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var award = await _context.Award.SingleOrDefaultAsync(m => m.AwardID == id);
            _context.Award.Remove(award);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AwardExists(int id)
        {
            return _context.Award.Any(e => e.AwardID == id);
        }
    }
}