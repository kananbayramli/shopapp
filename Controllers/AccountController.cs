using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.ui.EmailServices;
using shopapp.ui.Extensions;
using shopapp.ui.Identity;
using shopapp.ui.Models;

namespace shopapp.ui.Controllers
{
    [AutoValidateAntiforgeryToken] //All action
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ICartService _cartService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartService cartService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _cartService = cartService;
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
                TempData.Put("message", new AlertMessage() 
                { 
                    Title = "Invalid Username",
                    Message = "Your username is not true",
                    AlertType = "danger"
                });
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Account Confirmation",
                    Message = "Please confirm your account. We send link to your email.",
                    AlertType = "warning"
                });
                return View(model);
            }


            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl??"~/");
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Something went wrong :(",
                Message = "Your username or password is not true",
                AlertType = "danger"
            });
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
            TempData.Put("message", new AlertMessage()
            {
                Title = "Account closed",
                Message = "Your account closed securely",
                AlertType = "warning"
            });
            return Redirect("~/");
        }


        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Something went wrong :(",
                    Message = "Invalid token",
                    AlertType = "danger"
                });
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    //Cart Object created
                    _cartService.InitializeCart(user.Id);

                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Hi :)",
                        Message = "Your account has been confirmed",
                        AlertType = "success"
                    });
                    return View();
                }
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "Something went wrong :(",
                Message = "Your account has NOT been confirmed",
                AlertType = "warning"
            });
            return View();
        }


        public  IActionResult ForgotPassword() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return View();
            }

            var tokenCode =await _userManager.GeneratePasswordResetTokenAsync(user);

            // generate token
            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = tokenCode
            });

            // email confirm
            await _emailSender.SendEmailAsync(Email, "Hi, Reset your password :)", $"To reset your password, please <a href='https://localhost:5001{url}'>clik_here</a>");

            return View();
        }


        public IActionResult ResetPassword(string userId, string token) 
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new ResetPasswordModel { Token = token };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }


        public IActionResult AccessDenied() 
        {
            return View();
        }
    }
}