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
    public class RedemptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RedemptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            int page = 1, 
            int pageSize = 20)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            var redemptions = await _context.Redemption
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalCount = await _context.Redemption.CountAsync();

            return View(redemptions.ToPagedList(totalCount, page, pageSize));
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
            [Bind("Name,Description,PointsRequired")]
            Redemption redemption)
        {
            var dataUtils = new DataUtils();
            if (ModelState.IsValid)
            {
                redemption.RedemptionNumber = dataUtils.GenerateNumber(); 
                redemption.ModifiedDate = redemption.CreatedDate = DateTime.UtcNow;
                redemption.PointsRequired = redemption.PointsRequired;
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
            Bind("RedemptionID,Name,Description,PointsRequired")]
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
                        redemptionFromDatabase.Name = redemption.Name;
                        redemptionFromDatabase.Description = redemption.Description;
                        redemptionFromDatabase.ModifiedDate = DateTime.UtcNow;
                        redemptionFromDatabase.PointsRequired = redemption.PointsRequired;
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
            var viewModel = new RedemptionScanViewModel();
            viewModel.RedemptionNumber = $"{Request.Query["RedemptionNumber"]}";
            if (string.IsNullOrEmpty(viewModel.RedemptionNumber))
            {
                viewModel.HasRedemptionNumber = false;
            }
            else
            {
                viewModel.HasRedemptionNumber = true;
            }

            if (viewModel.HasRedemptionNumber)
            {
                var redemption = _context.Redemption.FirstOrDefault(i => i.RedemptionNumber == viewModel.RedemptionNumber);

                if (redemption == null)
                {
                    viewModel.RedemptionNumberInvalid = true;
                }
                else
                {
                    viewModel.Redemption = redemption;
                }
            }
            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogAction(string RedemptionNumber, string UserNumber)
        {
            var viewModel = new LogRedemptionJsonViewModel();
            try
            {

                //validate 
                var redemption = await _context.Redemption.FirstOrDefaultAsync(i => i.RedemptionNumber == RedemptionNumber);

                if (redemption == null)
                {
                    throw new ApplicationException("Redemption not found.");
                }

                var user = await _context.User.FirstOrDefaultAsync(i => i.UserNumber == UserNumber);

                if (user == null)
                {
                    throw new ApplicationException("User not found.");
                }

                var existingRedemptionByUser = await _context.RedemptionLog.FirstOrDefaultAsync(i =>
                                                    i.UserID == user.UserID &&
                                                    i.RedemptionID == redemption.RedemptionID);
                if (existingRedemptionByUser != null)
                {
                    throw new ApplicationException("User has already Redeemed this.");
                }

                //check if we have enough points
                int pointsShort = 0;
                int userPoints = PointsUtils.GetPointsForUser(user.UserID, _context);
                int pointsRequired = redemption.PointsRequired;
                bool hasEnoughPoints = false;

                pointsShort = pointsRequired - userPoints;

                if(pointsShort <= 0)
                {
                    hasEnoughPoints = true;
                    pointsShort = 0;
                }




                if (hasEnoughPoints)
                {
                    //good to go!
                    var redemptionLog = new RedemptionLog();
                    redemptionLog.RedemptionID = redemption.RedemptionID;
                    redemptionLog.CreatedDate = DateTime.UtcNow;
                    redemptionLog.ModifiedDate = redemptionLog.CreatedDate;
                    redemptionLog.UserID = user.UserID;
                    _context.Add(redemptionLog);
                    await _context.SaveChangesAsync();
                }

                

                viewModel.ResponseData.PointsShort = pointsShort;
                viewModel.ResponseData.Success = hasEnoughPoints;
                viewModel.ResponseData.PointsBalance = PointsUtils.GetPointsForUser(user.UserID, _context);
                viewModel.ResponseData.UserNumber = user.UserNumber;

            }
            catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
                viewModel.ResponseData = null;
            }

            return new JsonResult(viewModel);

        }
    }
}
