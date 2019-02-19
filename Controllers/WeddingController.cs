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
    public class WeddingController : Controller
    {
        private DojoContext dbContext;
        public WeddingController(DojoContext context)
        {
            dbContext = context;
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("User") == null)
                return Redirect("/login");
            
            
            List<Wedding> expired = dbContext.weddings
            .Where(w => DateTime.Compare(w.Date, DateTime.Now) < 0)
            .ToList();
            foreach (Wedding wedding in expired)
            {
                dbContext.Remove(wedding);
            }
            dbContext.SaveChanges();

            List<Wedding> weddings = dbContext.weddings
            .Include(w => w.GuestList)
            .ThenInclude(gl => gl.Guest)
            .ToList();

            ViewBag.User = HttpContext.Session.GetInt32("User");
            return View(weddings);
        }

        [HttpGet("weddings/new")]
        public IActionResult NewWedding()
        {
            if (HttpContext.Session.GetInt32("User") == null)
                return Redirect("/login");

            return View();
        }

        [HttpPost("weddings")]
        public IActionResult CreateWedding(Wedding wedding)
        {
            if (HttpContext.Session.GetInt32("User") == null)
                return Redirect("/login");

            wedding.UserId = (int)HttpContext.Session.GetInt32("User");

            if (ModelState.IsValid)
            {
                bool otherWed = dbContext.weddings
                .Any(w => w.UserId == (int)HttpContext.Session.GetInt32("User"));
                if (otherWed)
                {
                    ModelState.AddModelError("Wedder1", "You already have a wedding, bub");
                    return View("NewWedding");
                }

                dbContext.Add(wedding);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("NewWedding");
        }

        [HttpGet("weddings/{id}")]
        public IActionResult ShowWedding(int id)
        {
            if (HttpContext.Session.GetInt32("User") == null)
                return Redirect("/login");
            
            Wedding wedding = dbContext.weddings
            .Include(w => w.GuestList)
            .ThenInclude(gl => gl.Guest)
            .FirstOrDefault(w => w.WeddingId == id);

            return View(wedding);
        }

        [HttpGet("weddings/{id}/delete")]
        public IActionResult DeleteWedding(int id)
        {
            if (HttpContext.Session.GetInt32("User") == null)
                return Redirect("/login");

            dbContext.weddings
            .Remove(
            dbContext.weddings
            .FirstOrDefault(w => w.WeddingId == id)
            );
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("weddings/{id}/rsvp")]
        public IActionResult CreateRSVP(int id)
        {
            if (HttpContext.Session.GetInt32("User") == null)
                return Redirect("/login");

            RSVP rsvp = new RSVP
            {
                WeddingId = id,
                UserID = (int)HttpContext.Session.GetInt32("User")
            };
            dbContext.Add(rsvp);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("weddings/{id}/rsvp/delete")]
        public IActionResult DeleteRSVP(int id)
        {
            if (HttpContext.Session.GetInt32("User") == null)
                return Redirect("/login");

            dbContext.Remove(
            dbContext.rsvps
            .FirstOrDefault(r => r.UserID == HttpContext.Session.GetInt32("User") && r.WeddingId == id));
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }

    }
}