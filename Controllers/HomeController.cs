using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using WeddingPlanner.Models;


namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {   
        private DojoContext dbContext;
        public HomeController(DojoContext context)
        {
            dbContext = context;
        }
        
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult ShowLogin()
        {
            return View("login");
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("Index");
                }
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                user.Password = hasher.HashPassword(user, user.Password);

                dbContext.Add(user);
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("User", user.UserId);
                return Redirect("/dashboard");
            }
            return View("Index");
        }

        [HttpPost("Login")]
        public IActionResult Login(Login user)
        {
            if (ModelState.IsValid)
            {
                User userDB = dbContext.users.FirstOrDefault(u => u.Email == user.Email);
                if (userDB == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("login");
                }
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                var verifyPassword = hasher.VerifyHashedPassword(userDB, userDB.Password, user.Password);
                if (verifyPassword == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("login");
                }
                HttpContext.Session.SetInt32("User", userDB.UserId);
                return Redirect("/dashboard");
            }
            return View("Login");
        }

        [HttpGet("success")]
        public string Success()
        {
            if (HttpContext.Session.GetString("User") == null)
                return "failure";
            return "success";
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("login");
        }
    }
}
