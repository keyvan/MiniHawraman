using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using MiniHawraman.Components;
using MiniHawraman.Core.Components;
using MiniHawraman.Core.Models;
using MiniHawraman.Core.Services.Implementations;
using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(ISiteService siteService, IFormsAuthentication formsAuth, IUserService userService)
            : base(siteService)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            this.UserService = userService;
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        public IUserService UserService
        {
            get;
            private set;
        }


        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            ViewBag.VerificationCodeEnabled = false;

            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (string.IsNullOrEmpty(model.VerificationCode))
                Util.LoginNotification(this.UserService.GetUser("admin").Email, Request.UserHostAddress, model.UserName);

            ViewBag.VerificationCodeEnabled = false;

            bool valid = false;

            if (ModelState.IsValid)
            {
                if (this.UserService.ValidateUser(model.UserName, model.Password))
                {
                    if (string.IsNullOrEmpty(model.VerificationCode))
                    {
                        ViewBag.VerificationCodeEnabled = true;

                        string code = RandomCodeGenerator.GenerateCode();
                        this.UserService.SetValidationCode(model.UserName, code);

                        SmsManager.SendTextMessage(string.Format("Your login verification code for MashThis.IO is {0}.", code));

                        View(model);
                    }
                    else
                    {
                        if (this.UserService.GetValidationCode(model.UserName) == model.VerificationCode)
                        {
                            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "The verification code is incorrect.");
                            ViewBag.VerificationCodeEnabled = true;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            if (valid)
            {

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuth.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions result in password not being changed.")]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (!ValidateChangePassword(currentPassword, newPassword, confirmPassword))
            {
                return View();
            }

            try
            {
                if (UserService.ChangePassword(User.Identity.Name, currentPassword, newPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("_FORM", "The current password is incorrect or the new password is invalid.");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("_FORM", "The current password is incorrect or the new password is invalid.");
                return View();
            }
        }

        [Authorize]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
        }

        #region Validation Methods

        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (String.IsNullOrEmpty(currentPassword))
            {
                ModelState.AddModelError("currentPassword", "You must specify a current password.");
            }

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "The new password and confirmation password do not match.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateLogOn(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "You must specify a password.");
            }
            if (!UserService.ValidateUser(userName, password))
            {
                ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateRegistration(string userName, string email, string password, string confirmPassword)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "You must specify an email address.");
            }

            if (!String.Equals(password, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "The new password and confirmation password do not match.");
            }
            return ModelState.IsValid;
        }

        #endregion
    }
}
