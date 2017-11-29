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
    using Indspire.Soaring.Engagement.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = RoleNames.Administrator)]
    public class AwardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AwardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Award
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            var awards = await _context.Award
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalCount = await _context.Award.CountAsync();

            return View(awards.ToPagedList(totalCount, page, pageSize));
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
        public async Task<IActionResult> Create([Bind("Name,Description,Points")] Award award)
        {
            var dataUtils = new DataUtils();
            if (ModelState.IsValid)
            {
                award.CreatedDate = DateTime.UtcNow;
                award.ModifiedDate = award.CreatedDate;
                award.Deleted = false;
                award.EventNumber = dataUtils.GenerateNumber();

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
        public async Task<IActionResult> Edit(int id, [Bind("AwardID,Name,Description,Points")] Award award)
        {
            if (id != award.AwardID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var awardFromDB = _context.Award
                        .FirstOrDefault(i => i.AwardID == award.AwardID);

                    if (awardFromDB != null)
                    {
                        awardFromDB.Name = award.Name;
                        awardFromDB.Description = award.Description;
                        awardFromDB.Points = award.Points;
                        awardFromDB.ModifiedDate = DateTime.UtcNow;
                    }


                    _context.Update(awardFromDB);
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogAction(string AwardNumber, string UserNumber)
        {
            var viewModel = new LogActionJsonViewModel();
            try
            {
               
                //validate 
                var award = await _context.Award.FirstOrDefaultAsync(i => i.EventNumber == AwardNumber);

                if(award == null)
                {
                    throw new ApplicationException("Award not found.");
                }

                var user = await _context.User.FirstOrDefaultAsync(i => i.UserNumber == UserNumber);

                if(user == null)
                {
                    throw new ApplicationException("User not found.");
                }

                var existingAwardLogByThisUser = await _context.AwardLog.FirstOrDefaultAsync(i =>
                                                    i.UserID == user.UserID &&
                                                    i.AwardID == award.AwardID);
                if(existingAwardLogByThisUser != null)
                {
                    throw new ApplicationException("User has already been Awarded points for this action.");
                }

                //good to go!

                var awardLog = new AwardLog();
                awardLog.AwardID = award.AwardID;
                awardLog.CreatedDate = DateTime.UtcNow;
                awardLog.ModifiedDate = awardLog.CreatedDate;
                awardLog.Points = award.Points;
                awardLog.UserID = user.UserID;

                _context.Add(awardLog);

                await _context.SaveChangesAsync();

                viewModel.ResponseData.PointsAwarded = awardLog.Points;
                viewModel.ResponseData.PointsBalance = PointsUtils.GetPointsForUser(user.UserID, _context);
                viewModel.ResponseData.UserNumber = user.UserNumber;

            } catch(Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
                viewModel.ResponseData = null;
            }

            return new JsonResult(viewModel);

        }
    }
}
