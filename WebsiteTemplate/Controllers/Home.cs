using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using SystemPlus;
using SystemPlus.Web.Email;
using WebsiteTemplate.Core;
using WebsiteTemplate.Data;
using WebsiteTemplate.Models;
using WebsiteTemplate.ViewModels;
using WebsiteTemplate.ViewModels.Home;

namespace WebsiteTemplate.Controllers
{
    [AllowAnonymous]
    public class Home : MyBaseController
    {
        readonly IEmailSender emailer;
        readonly ILogger logger;

        public Home(UserManager<User> userManager, DataAccess dataAccess, IEmailSender emailer, ILogger<Home> logger)
            : base(userManager, dataAccess)
        {
            this.emailer = emailer;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            HomeModel model = new HomeModel()
            {

            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult About()
        {
            return RedirectToAction(nameof(Contact));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult TermsOfService()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult PrivacyStatement()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Error()
        {
            IExceptionHandlerFeature feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Exception ex = feature?.Error;

            if (ex != null)
            {
                User user = await GetUserAsync();

                if (user != null)
                {
                    ex.AddData("User", user.Email);
                    ex.AddData("User id", user.Id);
                }
            }

            logger.LogError(ex, "Error");

            return ErrorMessage(new ErrorModel() { Title = "Oops! an error has occured", Text = ex?.Message });
        }

        [AllowAnonymous]
        [Route("Home/Status/{statusCode}")]
        public async Task<IActionResult> Status(int statusCode)
        {
            User user = await GetUserAsync();
            HttpStatusCode status = (HttpStatusCode)statusCode;

            // string s = this.HttpContext.Request.Path;
            //Logger.Default.LogError(string.Format("{0}, {1}", user?.Email, status));

            return ErrorMessage(new ErrorModel() { Title = "Http server error", Text = status.ToString() });
        }

        [AllowAnonymous]
        [Route("/autodiscover/autodiscover.xml")]
        public IActionResult Autodiscover()
        {
            // handle office 365 requests
            return new EmptyResult();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendFeedback(FeedbackModel model)
        {
            User user = await GetUserAsync();

            string message = string.Format("From: {0}\r\nUser id: {1}\r\n\r\n{2}", user.Email, user.Id, model.Message);

            await emailer.SendEmailAsync(GlobalSettings.FeedbackEmail, GlobalSettings.FeedbackEmail, user.Email, model.Subject, message, false);

            return Message(new MessageModel() { Title = "Thanks!", Text = "Your feedback has been sent." });
        }
    }
}
