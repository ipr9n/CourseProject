using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using LogicLayer.Interfaces;
using Last.Models;
using LogicLayer.Infrastructure;
using System.Threading.Tasks;
using Last.Hubs;
using Last.Filters;
using LogicLayer.Services;

namespace Last.Controllers
{
    [LocalizeLanguage]
    [UnblockedOnly]
    public class HomeController : Controller
    {
        private ServiceCreator serviceCreator = new ServiceCreator();

        private ICollectionService CollectionService
        {
            get
            {
                return serviceCreator.CreateCollectionService();

            }
        }

        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }


        public ActionResult Index(string id)
        {
            if (User.Identity.IsAuthenticated && !UserService.IsUserExist(User.Identity.GetUserId()))
                return RedirectToAction("Logout", "Account");

            if (!UserService.IsUserExist(id))
                id = User.Identity.GetUserId();

            ViewBag.MyId = User.Identity.GetUserId();

            if (UserService.IsBlocked(User.Identity.GetUserId()))
                return View("Error", "",null);

            var userDetails = UserService.GetProfileInformation(id);

            return View(userDetails);
        }

        public async Task<ActionResult> GetAdmin()
        {
           await UserService.AddUserToRole(User.Identity.GetUserId(),"admin");
                return RedirectToAction("AdminPanel");
        }

        [Authorize(Roles ="admin")]
        public ActionResult AdminPanel()
        {
            return View(UserService.GetUsersProfiles());
        }

        [HttpPost]
        public async Task<ActionResult> BlockForm(string[] checks)
        {
            if (checks != null)
                await UserService.BlockUsers(checks);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> UnBlockForm(string[] checks)
        {
            if (checks != null)
                await UserService.UnBlockUsers(checks);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult UserDeleteForm(string[] checks)
        {
            if (checks != null)
                UserService.DeleteUsers(checks);    

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> AddAdminsForm(string[] checks)
        {
            if (checks != null)
            {
                await UserService.AddUsersToRole(checks, "admin");
            }

            return RedirectToAction("AdminPanel", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> RemoveAdminsForm(string[] checks)
        {
            if (checks != null)
            {
               await UserService.RemoveUsersRole(checks, "admin");
            }

            return RedirectToAction("AdminPanel", "Home");
        }

  
        [HttpGet]
        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            List<string> cultures = new List<string>() { "ru", "en"};

            if (Request.Cookies["lang"] == null)
            {
                Response.Cookies["lang"].Value = "ru";
            }
            else
            {
                if (Request.Cookies["lang"].Value == "en")
                {
                    Response.Cookies["lang"].Value = "ru";
                }
                else if (Request.Cookies["lang"].Value == "ru")
                {
                    Response.Cookies["lang"].Value = "en";
                }
            }

            return Redirect(returnUrl);
        }

        public ActionResult Users()
        {
            if (UserService.IsBlocked(User.Identity.GetUserId()))
                return View("Error", "", null);

            return View(UserService.GetUsersProfiles());
        }

        public ActionResult ChangeTheme()
        {
            if (Request.Cookies["theme"] == null)
            {
                Response.Cookies["theme"].Value = "dark";
            }
            else
            {
                if (Request.Cookies["theme"].Value == "dark")
                {
                    Response.Cookies["theme"].Value = "light";
                }
                else if (Request.Cookies["theme"].Value == "light")
                {
                    Response.Cookies["theme"].Value = "dark";
                }
            }

            return RedirectToAction("Index");
        }
    }
}