﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteTemplate.Data;
using WebsiteTemplate.Models;

namespace WebsiteTemplate.Controllers
{
    [Authorize]
    //[Area("")]
    public class Template : MyBaseController
    {
        public Template(UserManager<User> userManager, Database database)
            : base(userManager, database)
        {

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
