namespace Indspire.Soaring.Engagement.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Indspire.Soaring.Engagement.ViewModels;

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

            var topAwarded = await _context.AwardLog
                  .GroupBy(i => i.AwardID)
                  .Select(i => new AwardsRow()
                  {
                      AwardNumber = i.FirstOrDefault().Award.AwardNumber,
                      TimesAwards = i.Count(),
                      Name = i.FirstOrDefault().Award.Name,
                      Description = i.FirstOrDefault().Award.Description
                  })
                  .OrderByDescending(i => i.TimesAwards)
                  .Take(25)
                  .ToListAsync();

            var topUsers = await _context.AwardLog
                .GroupBy(i => i.UserID)
                .Select(i => new AttendeeRow()
                {
                    UserNamber = i.FirstOrDefault().User.UserNumber,
                    ExternalId = i.FirstOrDefault().User.ExternalID,
                    Points = i.Sum(p => p.Points)
                })
                .OrderByDescending(i => i.Points)
                .Take(25)
                .ToListAsync();

            var topRedemptions = await _context.RedemptionLog
                .GroupBy(i => i.RedemptionID)
                .Select(i => new RedemptionsRow()
                {
                    RedemptionNumber = i.FirstOrDefault().Redemption.RedemptionNumber,
                    TimesRedeemed = i.Count(),
                    Name = i.FirstOrDefault().Redemption.Name,
                    Description = i.FirstOrDefault().Redemption.Description
                })
                .OrderByDescending(i => i.TimesRedeemed)
                .Take(25)
                .ToListAsync();

            var dashboardReports = new DashboardReports();
            dashboardReports.AwardsList = topAwarded;
            dashboardReports.AttendeeList = topUsers;
            dashboardReports.RedemptionsList = topRedemptions;

            return View("~/Views/Admin/Admin.cshtml", dashboardReports);
        }
    }
}