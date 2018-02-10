// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Controllers
{
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Models;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        public IInstanceSelector InstanceSelector { get; set; }

        public ApplicationDbContext DatabaseContext { get; set; }
    }
}
