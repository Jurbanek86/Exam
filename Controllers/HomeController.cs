using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext dbContext;
        public HomeController(HomeContext context)
        {
            dbContext = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User register)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any( u => u.Email == register.Email))
                {
                    ModelState.AddModelError("Email", "That email is already in use");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    register.Password = hasher.HashPassword(register, register.Password);

                    dbContext.Users.Add(register);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetString("UserEmail", register.Email);
                    return RedirectToAction("Dashboard", "Job");


                }
            }
            else
            {
                return View("Index");
            }
        }

        public IActionResult Exam(ExamUser Exam)
        {
            if(ModelState.IsValid)
            {
                User userInDb = dbContext.Users.FirstOrDefault(u => u.Email == Exam.ExamEmail);
                if( userInDb ==null )
                {
                    ModelState.AddModelError("ExamEmail", "Invalid Email or Password" );
                }
                PasswordHasher<ExamUser> hasher = new PasswordHasher<ExamUser>();
                var result = hasher.VerifyHashedPassword(Exam, userInDb.Password, Exam.ExamPassword);
                if( result == 0)
                {
                    ModelState.AddModelError("ExamEmail", "Invalid Email or Password" );
                    return View("SignIn");
                }
                HttpContext.Session.SetString("UserEmail", Exam.ExamEmail);
                return RedirectToAction("Dashboard", "Job");

            }
            else
            {
                
                return View("SignIn");
            }
            
        }

        

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");

        }
       



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
