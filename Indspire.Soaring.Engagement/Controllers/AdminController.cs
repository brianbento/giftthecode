namespace Indspire.Soaring.Engagement.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Index()
        {
            var topAwardPoints = await _context.AwardLog
                .GroupBy(i => i.AwardID)
                .Select(i => new TopAward
                {
                    AwardID = i.Key,
                    TotalPoints = i.Sum(b => b.Points)
                })
                .OrderBy(i => i.TotalPoints)
                .Take(25)
                .ToListAsync();

            foreach (var topAwardPoint in topAwardPoints)
            {
                topAwardPoint.Award = await _context.Award.FirstOrDefaultAsync(
                    i => i.AwardID == topAwardPoint.AwardID);
            }

            //var topAwardPoints = await _context.RedemptionLog
            //    .GroupBy(i => i.UserID)
            //    .Select(i => new
            //    {
            //        UserID = i.Key,
            //        TotalPoints = i.Sum(b => )
            //    })
            //    .Take(25)
            //    .ToListAsync();

            return View("~/Views/Admin/Admin.cshtml", topAwardPoints);
        }
    }
}