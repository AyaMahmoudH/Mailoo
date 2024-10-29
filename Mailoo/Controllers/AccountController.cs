using Azure.Core;
using Mailo.Data;
using Mailo.Models;
using Mailoo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System;
using System.Security.Claims;

namespace Mailo.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = new User
                {
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    Password = model.Password,
                    Username = model.Username,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Gender = model.Gender,
                    UserType = model.UserType
                };

                try
                {
                    _context.Users.Add(account);
                    _context.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = $"{account.FName} {account.LName} registered successfully. Please Login.";
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Please enter unique Email or Password.");
                    return View(model);
                }
                return View();
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(x => (x.Username == model.UsernameOrEmail || x.Email == model.UsernameOrEmail));
                if (user != null && user.Password == model.Password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Name", user.FName),
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim(ClaimTypes.Role, user.UserType.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Username/Email or Password is not correct");
                }
            }
            return View(model);
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Edit()
        {
            var email = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                var model = new EditUserViewModel
                {
                    FName = user.FName,
                    LName = user.LName,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Email = user.Email, // Displayed as read-only
                    Username = user.Username, // Displayed as read-only
                    CurrentPassword = user.Password, // Display the current password directly
                    Gender = user.Gender,
                    UserType = user.UserType
                };
                return View(model);
            }
            return NotFound();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    user.FName = model.FName;
                    user.LName = model.LName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Address = model.Address;

                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        if (user.Password == model.CurrentPassword) // Check if displayed password matches the stored password
                        {
                            if (model.NewPassword == model.ConfirmNewPassword)
                            {
                                user.Password = model.NewPassword;
                            }
                            else
                            {
                                ModelState.AddModelError("", "New Password and Confirm Password do not match.");
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Current Password is incorrect.");
                            return View(model);
                        }
                    }

                    _context.SaveChanges();
                    ViewBag.Message = "Profile updated successfully!";

                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        return RedirectToAction("Login");
                    }

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    ViewBag.Message = "Check your email for the password reset link.";
                    return View();
                }
                ModelState.AddModelError("", "Email not found.");
            }
            return View(model);
        }
   

    public async Task SendResetPasswordEmail(string email, string token)
    {
        var resetLink = Url.Action("ResetPassword", "Account", new { token }, Request.Scheme);

        var message = new MailMessage();
        message.To.Add(email);
        message.Subject = "Reset Password";
        message.Body = $"Please reset your password by clicking here: <a href=\"{resetLink}\">link</a>";
        message.IsBodyHtml = true;

        using (var smtp = new SmtpClient())
        {
            smtp.Host = "smtp.example.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("username", "password");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(message);
        }
    }
        public IActionResult ResetPassword(string token)
        {
            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.PasswordResetToken == model.Token); 

                if (user != null)
                {
                    user.Password = model.NewPassword; 
                    _context.SaveChanges();
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", "Invalid token.");
            }
            return View(model);
        }

    }
}
