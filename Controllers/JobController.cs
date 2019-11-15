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
    [Route("jobs")]
    public class JobController: Controller
    {
        private HomeContext dbContext;
        public JobController(HomeContext context)
        {
            dbContext = context;
        }
        
        [HttpGet("")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Signin", "Home");
            }
            else
            {
                User userInDb = dbContext.Users.Include(u => u.PlannedJob).FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserEmail"));
                ViewBag.User = userInDb;
                List<Job> AllJobs = dbContext.Jobs.Include(u => u.WorkerList).ThenInclude(r => r.Worker).Include( s => s.Planner).Where(d => d.Date > DateTime.Now).OrderBy(p => p.Date).ToList();
                return View(AllJobs);
            }
        }
        [HttpGet("New")]
        public IActionResult New()
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Signin", "Home");
            }
            else
            {
                User userInDb = dbContext.Users.Include(u => u.PlannedJob).FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserEmail"));
                ViewBag.User = userInDb;
                return View();
            }
            
        }
        [HttpPost("create")]
        public IActionResult Create(Job plan)
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Signin", "Home");
            }
            else
            {
                if(ModelState.IsValid)
                {
                    dbContext.Jobs.Add(plan);
                    dbContext.SaveChanges();
                    return Redirect($"show/{plan.JobId}");
                }
                else{
                    User userInDb = dbContext.Users.Include(u => u.PlannedJob).FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserEmail"));
                    ViewBag.User = userInDb;
                    return View("New");
                }
            }
        }
        [HttpGet("show/{jobId}")]
        public IActionResult Show(int jobId)
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Signin", "Home");
            }
            else
            {
                User userInDb = dbContext.Users.Include(u => u.PlannedJob).FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserEmail"));
                ViewBag.User = userInDb;
                Job show = dbContext.Jobs.Include(w => w.WorkerList).ThenInclude(r => r.Worker).Include(w => w.Planner).FirstOrDefault(w => w.JobId == jobId);
                return View(show);
            }
        }
        [HttpGet("rsvp/{jobId}/{userId}/{status}")]
        public IActionResult RSVP(int jobId, int userId, string status)
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Signin", "Home");
            }
            else
            {
                if(status == "add")
                {
                    RSVP response = new RSVP();
                    response.UserId = userId;
                    response.JobId = jobId;
                    dbContext.RSVPs.Add(response);
                    dbContext.SaveChanges();
                }
                if(status == "remove")
                {
                    RSVP remove = dbContext.RSVPs.FirstOrDefault(r => r.JobId == jobId && r.UserId == userId);
                    dbContext.RSVPs.Remove(remove);
                    dbContext.SaveChanges();
                    

                }
                return RedirectToAction("Dashboard");
            }
        }
        [HttpGet("destroy/{jobId}")]
        public IActionResult Destroy(int jobId)
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Signin", "Home");
            }
            else
            {
                Job remove = dbContext.Jobs.FirstOrDefault( w => w.JobId == jobId);
                dbContext.Jobs.Remove(remove);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
        }
        
    }
}