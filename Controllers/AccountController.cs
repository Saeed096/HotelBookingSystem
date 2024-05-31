using HotelBookingSystem.Interfaces;
using HotelBookingSystem.Models;
using HotelBookingSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBookingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly iUnitOfWork unitOfWork;

        public AccountController(iUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
      


        [HttpGet]
        public IActionResult register()
        {
  
            return View("register");
        }


        [HttpGet]
        public async Task<IActionResult> mailConfirmed(string email)
        {
            ApplicationUser? user = await unitOfWork.userManager.FindByEmailAsync(email);
            user.EmailConfirmed = true;
            await unitOfWork.userManager.UpdateAsync(user);
            return RedirectToAction("login");
        }


        [HttpPost]
        public async Task<IActionResult> register(RegisterViewModel model)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = model.userName,
                PasswordHash = model.password,
                PhoneNumber = model.phoneNumber,
                Email = model.Email?.Trim()
            };

            if (ModelState.IsValid)
            {
                IdentityResult result = new IdentityResult();
                try
                {
                    result = await unitOfWork.userManager.CreateAsync(applicationUser,
                    applicationUser.PasswordHash);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.StartsWith("Cannot insert duplicate key"))  
                        ModelState.AddModelError(string.Empty, "Already existing email");  

                    else { ModelState.AddModelError(string.Empty, ex.InnerException.Message); }
                }


                if (result.Succeeded)
                { 
                    await unitOfWork.userManager.AddToRoleAsync(applicationUser, "User");
                    return RedirectToAction("SendForceEmailConfirmationMail", "Mail", new { toEmail = model.Email });
                }


                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }

            }

            return View("register");

        }

        [HttpGet]
        public IActionResult login()
        {
            return View("login");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await unitOfWork.userManager.FindByNameAsync(model.userName);
                if (user != null)
                {
                    bool matched = await unitOfWork.userManager.CheckPasswordAsync(user, model.password);
                    if (matched)
                    {
                   
                        if (user.EmailConfirmed)
                        {
                            List<Claim> claims = new List<Claim>();
                            claims.Add(new Claim("name", model.userName));
                            claims.Add(new Claim("id", user.Id));
                            await unitOfWork.signInManager.SignInWithClaimsAsync(user, model.rememberMe, claims);
                            if(User.IsInRole("Admin"))
                            {
                            return RedirectToAction("getAll", "reservation");
                            }
                            return RedirectToAction("create", "reservation");
                        }
                        ModelState.AddModelError("", "Unconfirmed email");
                        return View("login");
                    }
                    ModelState.AddModelError("", "invalid password");
                    return View("login");
                }
            }
            ModelState.AddModelError("", "invalid user name");
            return View("login", model);
        }


        public async Task<IActionResult> logout()
        {
            await unitOfWork.signInManager.SignOutAsync();
            return RedirectToAction("login");
        }


        [HttpGet]
        public IActionResult forgotPassword()
        {
            return View("forgotPassword");
        }


        [HttpPost]
        public async Task<IActionResult> forgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Email = model?.Email.Trim();
                ApplicationUser? user = await unitOfWork.userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    string token = await unitOfWork.userManager.GeneratePasswordResetTokenAsync(user);
                    //  token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));         // saeed : search for decoding 
                    string? callBackUrl = Url.Action("resetPassword", "account", values: new { token, userName = user.UserName },
                        protocol: Request.Scheme);

                    return RedirectToAction("SendMail", "Mail",
                        routeValues: new { emailTo = user.Email, username = user.UserName, callBackUrl = callBackUrl });

                }
                ModelState.AddModelError("", "Email not existed");
                return View("forgotPassword", model);
            }
            return View("forgotPassword", model);
        }


        [HttpGet]
        public IActionResult ResetPassword([FromQueryAttribute] string userName, [FromQueryAttribute] string token)
        {
            ViewBag.UserName = userName;
            ViewBag.Token = token;
            return View("resetPassword");
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(resetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await unitOfWork.userManager.FindByNameAsync(model.userName);
                IdentityResult result = await unitOfWork.userManager
                       .ResetPasswordAsync(user, model.token, model.newPassword);


                if (result.Succeeded)
                    return View("login");

                else
                {
                    foreach (IdentityError error in result.Errors)
                    { ModelState.AddModelError(string.Empty, error.Description); }

                    return View("resetPassword", model);
                }
            }
            return View("resetPassword", model);
        }

    }
}
