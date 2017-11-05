using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Indspire.Soaring.Engagement.Models;
using Indspire.Soaring.Engagement.Data;
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
using Indspire.Soaring.Engagement.Utils;
using Indspire.Soaring.Engagement.Extensions;

namespace Indspire.Soaring.Engagement.Controllers
{
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
            //var topAwardPoints = await _context.AwardLog
            //    .GroupBy(i => i.AwardID)
            //    .Select(i => new
            //    {
            //        AwardID = i.Key,
            //        TotalPoints = i.Sum(b => b.Points)
            //    })
            //    .Take(25)
            //    .ToListAsync();

            //var topAwardPoints = await _context.RedemptionLog
            //    .GroupBy(i => i.UserID)
            //    .Select(i => new
            //    {
            //        UserID = i.Key,
            //        TotalPoints = i.Sum(b => b.)
            //    })
            //    .Take(25)
            //    .ToListAsync();

            return View("~/Views/Admin/Admin.cshtml");
        }
    }
}