using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetMvc.Contexts;
using DotNetMvc.Entities;
using DotNetMvc.Models;
using DotNetMvc.Services;

namespace DotNetMvc.Controllers
{
    public class DirectorsController : Controller
    {
        private MoviesContext db = new MoviesContext();

        private readonly DirectorService directorService;
        private readonly MovieService movieSerivce;

        public DirectorsController()
        {
            directorService = new DirectorService(db);
            movieSerivce = new MovieService(db);
        }

        // GET: Directors
        public ActionResult Index()
        {
            //return View(db.Directors.ToList());
            return View(directorService.GetQuery().ToList());
        }

        // GET: Directors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Director director = db.Directors.Find(id);
            DirectorModel director = directorService.GetQuery().SingleOrDefault(d => d.Id == id);

            if (director == null)
            {
                return HttpNotFound();
            }
            return View(director);
        }

        // GET: Directors/Create
        public ActionResult Create()
        {
            //return View();
            ViewBag.Movies = new MultiSelectList(movieSerivce.GetQuery().ToList(), "Id", "Name");
            return View(new DirectorModel());
        }

        // POST: Directors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Name,Surname,Retired")] Director director)
        //public ActionResult Create([Bind(Include = "Name,Surname,Retired,MovieIds")] DirectorModel director)
        //public ActionResult Create([Bind(Exclude = "Id,Movies")] DirectorModel director)
        public ActionResult Create(DirectorModel director)
        {
            if (ModelState.IsValid)
            {
                //db.Directors.Add(director);
                //db.SaveChanges();
                if (directorService.Add(director))
                    return RedirectToAction("Index");
                ViewData["Message"] = "İşlem sırasında hata meydana geldi!";
            }
            ViewBag.Movies = new MultiSelectList(movieSerivce.GetQuery().ToList(), "Id", "Name", director.MovieIds);
            return View(director);
        }

        // GET: Directors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Director director = db.Directors.Find(id);
            DirectorModel director = directorService.GetQuery().SingleOrDefault(d => d.Id == id);

            if (director == null)
            {
                return HttpNotFound();
            }

            ViewBag.Movies = new MultiSelectList(movieSerivce.GetQuery().ToList(), "Id", "Name", director.MovieIds);

            return View(director);
        }

        // POST: Directors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name,Surname,Retired")] Director director)
        public ActionResult Edit(DirectorModel director)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(director).State = EntityState.Modified;
                //db.SaveChanges();
                if (directorService.Update(director))
                    return RedirectToAction("Index");
                ViewBag.Message = "İşlem sırasında hata meydana geldi!";
            }
            ViewBag.Movies = new MultiSelectList(movieSerivce.GetQuery().ToList(), "Id", "Name", director.MovieIds);
            return View(director);
        }

        // GET: Directors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Director director = db.Directors.Find(id);
            DirectorModel director = directorService.GetQuery().SingleOrDefault(d => d.Id == id);

            if (director == null)
            {
                return HttpNotFound();
            }
            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Director director = db.Directors.Find(id);
            //db.Directors.Remove(director);
            //db.SaveChanges();
            if (directorService.Delete(id))
                return RedirectToAction("Index");
            return View("Error");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
