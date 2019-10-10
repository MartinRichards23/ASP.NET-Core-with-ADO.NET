using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebsiteTemplate.Data;
using WebsiteTemplate.Models;
using WebsiteTemplate.ViewModels;
using WebsiteTemplate.ViewModels.Home;

namespace WebsiteTemplate.Controllers
{
    [Controller]
    public abstract class MyBaseController : Controller
    {
        #region Fields

        protected UserManager<User> UserManager { get; }
        protected Database Database { get; }

        #endregion

        public MyBaseController(UserManager<User> userManager, Database database)
        {
            UserManager = userManager;
            Database = database;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await GetUser();
                AddUserData(user);
            }

            await base.OnActionExecutionAsync(context, next);
        }

        /// <summary>
        /// Redirects to page showing title and message
        /// </summary>
        [HttpGet]
        public IActionResult Message(MessageModel model)
        {
            return View("Message", model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ErrorMessage(ErrorModel model)
        {
            return View("Error", model);
        }

        public IActionResult RedirectToError(string errorMessage)
        {
            return RedirectToAction(nameof(Home.ErrorMessage), nameof(Home), new { area = "", errorMessage = errorMessage });
        }

        protected int GetUserId()
        {
            string idString = UserManager.GetUserId(User);

            if (!int.TryParse(idString, out int userId))
                throw new Exception("User id is not valid");

            return userId;
        }

        /// <summary>
        /// Get the logged in user for the current session
        /// </summary>
        protected async Task<User> GetUserAsync()
        {
            return await UserManager.GetUserAsync(User);
        }

        protected async Task<User> GetUser()
        {
            return await GetUserById(GetUserId());
        }

        protected async Task<User> GetUserById(int id)
        {
            return await Database.GetUserAsync(id, CancellationToken.None);
        }

        protected void AddUserData(User user)
        {
            if (user != null)
            {
                ViewBag.User = user;
            }
        }
    }
}
