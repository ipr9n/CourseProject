using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Last.Models;
using LogicLayer.DTO;
using System.Security.Claims;
using LogicLayer.Interfaces;
using LogicLayer.Infrastructure;
using Last.Filters;

namespace Last.Controllers
{
    [LocalizeLanguage]
    public class AccountController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login() => View("Login");

        public ActionResult Register() => View("Register");

        public ActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }


            // Sign in the user with this external login provider if the user already has a login

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model) // тут пользователь авторизуется
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password
                };

                ClaimsIdentity claim = await UserService.Authenticate(userDto);

                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if(string.IsNullOrWhiteSpace(model.AvatarUrl))
                {
                    model.AvatarUrl = "https://res.cloudinary.com/ipr9n/image/upload/v1600436131/png-transparent-customer-service-antichrist-customer-support-call-centre-incognito-service-silhouette-user_hifazz.png";
                }
                var userDto = new UserDTO()
                {
                    Email = model.Email, Password = model.Password, FirstName = model.FirstName,
                    SecondName = model.SecondName, Role = "user", AvatarUrl=model.AvatarUrl, Address=model.Address, Age=model.Age,Gender = model.Gender
                };

                OperationDetails operationDetails = await UserService.Create(userDto);

                if (operationDetails.Succedeed)
                {
                    ClaimsIdentity claim = await UserService.Authenticate(userDto);

                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);

                    return RedirectToAction("Index", "Home");

                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }

            return View(model);
        }
    }
}