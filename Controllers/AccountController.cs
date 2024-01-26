using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Shop_Express.Models;
using Shop_Express.Data;
using Shop_Express.ViewModels;
using Microsoft.AspNetCore.Identity;
using Shop_Express.Service;

namespace Shop_Express.Controllers
{
    public class AccountController : Controller
    {
        private JobbingContext _context;
        private readonly UserPasswordService _userPassword;

        public AccountController(JobbingContext context, UserPasswordService userPassword)
        {
            _context = context;
            _userPassword = userPassword;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (await IsEmailUnique(model.Email))
                {
                    var (hash, salt) = await _userPassword.HashPasswordAsync(model.Password);

                    User user = new User(model.Email);

                    if (await TryAssignRole(user, "Reader"))
                    {
                        var userPassword = new UserPassword(hash, salt, user);
                        user.UserPassword = userPassword;
                        _context.Users.Add(user);
                        try
                        {
                            await _context.SaveChangesAsync();
                            await Authenticate(user);

                            return RedirectToAction("Index", "Home");
                        }
                        catch
                        {
                            ModelState.AddModelError("", "Error saving user to database. Please try again.");
                        }
                    }
                    else
                        ModelState.AddModelError("", "Error assigning role to user.");
                }
                else
                    ModelState.AddModelError("Email", "This email address is already registered. Please choose a different one or login if it's yours.");
            }

            return View(model);
        }

        private async Task<bool> IsEmailUnique(string email)
        {
            return await _context.Users.AllAsync(u => u.Email != email);
        }

        private async Task<bool> TryAssignRole(User user, string roleName)
        {
            Role? userRole = await _context.Roles.SingleOrDefaultAsync(r => r.Name == roleName);

            if (userRole != null)
            {
                user.Role = userRole;
                return true;
            }

            return false;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = await _context.Users
                    .Include(u => u.Role)
                    .Include(u => u.UserPassword)
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null)
                {
                    var passwordHasher = new UserPasswordService();
                    var passwordVerificationResult = passwordHasher.VerifyPasswordAsync(model.Password, user.UserPassword.Hash, user.UserPassword.Salt);
                    if (passwordVerificationResult.Result is OkResult)
                    {
                        await Authenticate(user);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                    ModelState.AddModelError("Email", "Incorrect login and/or password");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private async Task Authenticate(User user)
        {
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name),
                };
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie",
                                    ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                    new ClaimsPrincipal(id));
            }
        }
    }
}
