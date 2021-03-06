﻿using Eventis.Models.Identity;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace Eventis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();

            var events = db.Events
                .OrderByDescending(c => c.Id)
                .Take(6)
                .Include(x => x.Contact)
                .Include(x => x.Author)
                .Include(x => x.Genre.Category)
                .Include(x => x.Genre)
                .ToList();

            return View(events);
        }

        public ActionResult ListAll(int page = 1, string user = null)
        {
            var db = new ApplicationDbContext();

            var eventsQuery = db.Events.AsQueryable();

            var pageSize = 6;

            if (user != null)
            {
                eventsQuery = eventsQuery
                    .Where(e => e.Author.Email == user);
            }

            var events = eventsQuery
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Contact)
                .Include(x => x.Author)
                .Include(x => x.Genre.Category)
                .Include(x => x.Genre)
                .ToList();
            ViewBag.Title = "All events";
            ViewBag.CurrentPage = page;

            return View(events);
        }

        public ActionResult Cities()
        {
            var db = new ApplicationDbContext();

            var city = db
                .Cities
                .Include(h => h.Halls).
                ToList();

            return View(city);
        }

        public ActionResult Categories()
        {
            var db = new ApplicationDbContext();

            var categories = db.Categories.Include(g => g.Genres).ToList();

            return View(categories);
        }
        public ActionResult Halls(int id , int page = 1)
        {
            
            var db = new ApplicationDbContext();
            int pageSize = 6;
            var eventsQuery = db.Events.AsQueryable().Where(g => g.HallId == id);

            var hall = db.Events
                .Where(e => e.HallId == id)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.Genre.Category)
                .Include(g => g.Genre)
                .Include(g => g.Hall)
                .ToList();
            var hallname = db.Halls.Where(g => g.Id == id);
            ViewBag.Title = hallname.First().Name;
            ViewBag.CurrentPage = page;

            return View(hall);
        }
        public ActionResult Genres(int id, int page = 1)
        {
            
            int pageSize = 6;

            var db = new ApplicationDbContext();
            var eventsQuery = db.Events.AsQueryable().Where(g=>g.GenreId ==id);
            var events = db.Events
                .Where(g => g.GenreId == id)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.Genre.Category)
                .Include(g => g.Genre)
                .Include(g => g.Hall)
                .ToList();
            ViewBag.CurrentPage = page;
            
            var genre = db.Genres.Where(g => g.Id == id);
            ViewBag.Title = genre.First().Name;
            return View(events);
            
        }

    }
}