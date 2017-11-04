using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Indspire.Soaring.Engagement.Models;

namespace Indspire.Soaring.Engagement.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = RoleNames.Administrator)]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Admin.cshtml");
        }
    }
}