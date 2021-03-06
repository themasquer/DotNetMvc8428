using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetMvc.Contexts;
using DotNetMvc.Models;
using DotNetMvc.Services;

namespace DotNetMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    [HandleError]
    public class MoviesReportController : Controller
    {
        private MoviesContext db;
        private MovieReportService movieReportService;

        public MoviesReportController()
        {
            db = new MoviesContext();
            movieReportService = new MovieReportService(db);
        }

        // GET: MoviesReport
        public ActionResult Index(int? OnlyMatchedValue = null)
        {
            List<MovieReportInnerJoinModel> innerJoinList = null;
            List<MovieReportLeftOuterJoinModel> outerJoinList = null;
            int onlyMaychedValue = OnlyMatchedValue ?? 1;
            if (onlyMaychedValue == 1)
                innerJoinList = movieReportService.GetInnerJoinQuery().ToList();
            else
                outerJoinList = movieReportService.GetLeftOuterJoinQuery().ToList();

            List<SelectListItem> onlyMatchedSelectListItems = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "1",
                    Text = "Yes"
                },
                new SelectListItem()
                {
                    Value = "0",
                    Text = "No"
                }
            };

            //return View(innerJoinModel);
            MovieReportViewModel model = new MovieReportViewModel()
            {
                InnerJoinList = innerJoinList,
                OuterJoinList = outerJoinList,
                OnlyMatchedSelectList = new SelectList(onlyMatchedSelectListItems, "Value", "Text")
            };
            return View(model);
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