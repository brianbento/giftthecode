// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Controllers
{
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class BaseController : Controller
    {
        public IInstanceSelector InstanceSelector { get; set; }
    }
}
