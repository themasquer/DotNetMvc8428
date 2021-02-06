using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetMvc.Contexts;

namespace DotNetMvc.Controllers
{
    public class MoviesController : Controller
    {
        // CRUD: Create, Read, Update, Delete

        MoviesContext db = new MoviesContext();

        // GET: Movies
        public ActionResult Index()
        {
            var movies = db.Movies.ToList();
            return View(movies);
        }
    }
}