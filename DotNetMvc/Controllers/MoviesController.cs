using System;
using System.Collections.Generic;
using DotNetMvc.Contexts;
using DotNetMvc.Services;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DotNetMvc.Models;

namespace DotNetMvc.Controllers
{
    public class MoviesController : Controller
    {
        // CRUD: Create, Read, Update, Delete
        private readonly MoviesContext _db;
        private readonly MovieService _movieService;

        public MoviesController()
        {
            _db = new MoviesContext();
            _movieService = new MovieService(_db);
        }

        // GET: Movies
        public ViewResult Index()
        {
            var movies = _db.Movies.ToList();
            return View(movies);
        }

        public ActionResult List()
        {
            var movies = _movieService.GetQuery().ToList();
            return View("List", movies);
        }

        [HttpGet] // Action Method Selector, eğer yazılmazsa default'u HttpGet'tir
        public ViewResult Create()
        {
            List<int> years = new List<int>();
            for (int year = DateTime.Now.Year + 1; year >= 1930; year--)
            {
                years.Add(year);
            }
            ViewBag.Years = years;
            //return new ViewResult();
            return View();
        }

        [HttpPost]
        public ActionResult Create(string Name, double? BoxOfficeReturn, string ProductionYear)
        {
            //return Content("Movie created: " + "Name = " + Name + ", BoxOfficeReturn = " + BoxOfficeReturn + ", ProductionYear = " + ProductionYear);
            if (string.IsNullOrWhiteSpace(Name) || Name.Length > 250)
                return Content("Name must not be empty and name must have maximum 250 characters.");
            MovieModel model = new MovieModel()
            {
                Name = Name,
                BoxOfficeReturn = BoxOfficeReturn,
                ProductionYear = ProductionYear
            };
            bool result = _movieService.Add(model);
            if (result)
                return RedirectToAction("List");
            return Content("İşlem sırasında hata meydana geldi!");
        }

        public ActionResult Details(int? id)
        {
            //if (id == null)
            if (!id.HasValue)
            {
                //return View("Error");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // 400
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Id is required!");
            }

            //MovieModel model = _movieService.GetQuery().SingleOrDefault(m => m.Id == id);
            MovieModel model = _movieService.GetQuery().SingleOrDefault(m => m.Id == id.Value);
            if (model == null)
            {
                //return View("Error");
                //return new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
                //return new HttpNotFoundResult();
                return HttpNotFound();
            }

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            MovieModel model = _movieService.GetQuery().SingleOrDefault(m => m.Id == id);
            if (model == null)
                return HttpNotFound();
            List<int> years = new List<int>();
            for (int year = DateTime.Now.Year + 1; year >= 1930; year--)
            {
                years.Add(year);
            }
            List<SelectListItem> yearSelectListItems = years.Select(y => new SelectListItem()
            {
                Value = y.ToString(),
                Text = y.ToString()
            }).ToList();
            SelectList yearSelectList = new SelectList(yearSelectListItems, "Value", "Text", model.ProductionYear);
            //ViewBag.Years = yearSelectList;
            ViewData["Years"] = yearSelectList;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovieModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = _movieService.Update(model);
                if (result)
                    return RedirectToAction("List");
                return View("Error");
            }

            List<int> years = new List<int>();
            for (int year = DateTime.Today.Year + 1; year >= 1930; year--)
            {
                years.Add(year);
            }
            List<SelectListItem> yearSelectListItems = years.Select(y => new SelectListItem()
            {
                Value = y.ToString(),
                Text = y.ToString()
            }).ToList();
            SelectList yearSelectList = new SelectList(yearSelectListItems, "Value", "Text", model.ProductionYear);
            ViewBag.Years = yearSelectList;
            return View(model);
        }

        //public ContentResult Edit()
        //{
        //    //return new ContentResult()
        //    //{
        //    //    Content = "Edit View"
        //    //};
        //    string xml = "<Movies>";
        //    xml += "<Movie>";
        //    xml += "<Name>Avatar</Name>";
        //    xml += "<ProductionYear>2009</ProductionYear>";
        //    xml += "<BoxOfficeReturn>1000000</BoxOfficeReturn>";
        //    xml += "</Movie>";
        //    xml += "<Movie>";
        //    xml += "<Name>Sherlock Holmes</Name>";
        //    xml += "<ProductionYear>2009</ProductionYear>";
        //    xml += "<BoxOfficeReturn></BoxOfficeReturn>";
        //    xml += "</Movie>";
        //    xml += "</Movies>";
        //    //return Content("<span style=\"color: blue;\">Edit View</span>");
        //    return Content(xml, "application/xml");
        //}

        //public ActionResult Delete()
        //{
        //    return new EmptyResult();
        //}

        //public RedirectResult Details()
        //{
        //    //return new RedirectResult("https://www.google.com", true);
        //    //return Redirect(""https://www.google.com"");
        //    return RedirectPermanent("https://www.google.com");
        //}

        //public string Test()
        //{
        //    return "Test";
        //}
    }
}