using Indspire.Soaring.Engagement.Data;
using Indspire.Soaring.Engagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Indspire.Soaring.Engagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToActionPermanent(
                "Login", 
                "Account",
                new { returnUrl = "/user" });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
