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
    [HandleError]
    public class ReviewsController : Controller
    {
        private MoviesContext db = new MoviesContext();
        private ReviewService reviewService;
        private MovieService movieService;

        public ReviewsController()
        {
            reviewService = new ReviewService(db);
            movieService = new MovieService(db);
        }

        // GET: Reviews
        //[HandleError]
        public ActionResult Index()
        {
            //var reviews = db.Reviews.Include(r => r.Movie);
            //return View(reviews.ToList());
            return View(reviewService.GetQuery().ToList());

            //throw new Exception("Index test hatası!");
        }

        // GET: Reviews/Details/5
        //[HandleError]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Review review = db.Reviews.Find(id);
            ReviewModel review = reviewService.GetQuery().SingleOrDefault(r => r.Id == id);

            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        //[HandleError]
        [Authorize]
        public ActionResult Create()
        {
            //ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name");
            //return View();

            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name");
            ReviewModel model = new ReviewModel();
            reviewService.FillAllRatings(model);
            return View(model);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        //public ActionResult Create([Bind(Include = "Id,Content,Rating,Reviewer,MovieId")] Review review)
        public ActionResult Create(ReviewModel review)
        {
            if (ModelState.IsValid)
            {
                //db.Reviews.Add(review);
                //db.SaveChanges();
                reviewService.Add(review);

                return RedirectToAction("Index");
            }

            //ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name", review.MovieId);
            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name");
            reviewService.FillAllRatings(review);

            return View(review);
        }

        // GET: Reviews/Edit/5
        [Authorize(Users = "leo@alsac.com,angel@alsac.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Review review = db.Reviews.Find(id);
            ReviewModel review = reviewService.GetQuery().SingleOrDefault(r => r.Id == id);

            if (review == null)
            {
                return HttpNotFound();
            }

            //ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name", review.MovieId);
            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name", review.MovieId);
            reviewService.FillAllRatings(review);

            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "leo@alsac.com,angel@alsac.com")]
        //public ActionResult Edit([Bind(Include = "Id,Content,Rating,Reviewer,MovieId")] Review review)
        public ActionResult Edit(ReviewModel review)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(review).State = EntityState.Modified;
                //db.SaveChanges();
                reviewService.Update(review);

                return RedirectToAction("Index");
            }

            //ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name", review.MovieId);
            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name", review.MovieId);
            reviewService.FillAllRatings(review);

            return View(review);
        }

        //// GET: Reviews/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Review review = db.Reviews.Find(id);
        //    if (review == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(review);
        //}

        // POST: Reviews/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Review review = db.Reviews.Find(id);
        //    db.Reviews.Remove(review);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //[Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            reviewService.Delete(id.Value);
            return RedirectToAction("Index");
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
