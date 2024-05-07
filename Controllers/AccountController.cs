using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using shopapp.ui.EmailServices;
using shopapp.ui.Identity;
using shopapp.ui.Models;

namespace shopapp.ui.Controllers
{
    //[AutoValidateAntiforgeryToken] //All action
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult Login(string ReturnUrl=null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //var user = await _userManager.FindByNameAsync(model.UserName);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                CreateMessage("Your username is not true", "danger");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                CreateMessage("Please confirm your account. We send link to your email.", "warning");
                return View(model);
            }


            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl??"~/");
            }
            CreateMessage("Your username or password is not true", "danger");
            return View(model);
        }




        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        { 
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // generate token
                var tokenCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new{
                    userId = user.Id,
                    token = tokenCode
                });
                // email confirm
                await _emailSender.SendEmailAsync(model.Email, "Hi, Confirm your email :)", $"To confirm your email <a href='https://localhost:5001{url}'>clik_here</a>");
                return RedirectToAction("Login", "Account");
            }

            //ModelState.AddModelError("RePassword", "Tekrar parol duz deyil");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }


        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                CreateMessage("Invalid token", "danger");
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    CreateMessage("Your account has been confirmed", "success");
                    return View();
                }
            }
            CreateMessage("Your account has NOT been confirmed", "warning");
            return View();
        }



        private void CreateMessage(string message, string alerttype)
        {
            var obj = new AlertMessage()
            {
                Message = message,
                AlertType = alerttype
            };

            TempData["message"] = JsonConvert.SerializeObject(obj);
        }
    }
}