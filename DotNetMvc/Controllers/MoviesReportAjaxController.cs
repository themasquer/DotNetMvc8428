using DotNetMvc.Contexts;
using DotNetMvc.Models;
using DotNetMvc.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace DotNetMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    [HandleError]
    public class MoviesReportAjaxController : Controller
    {
        private MoviesContext db;
        private MovieReportService movieReportService;

        public MoviesReportAjaxController()
        {
            db = new MoviesContext();
            movieReportService = new MovieReportService(db);
        }

        // GET: MoviesReportAjax
        public ActionResult Index(MovieReportViewModel viewModel)
        {
            List<MovieReportInnerJoinModel> innerJoinList = null;
            List<MovieReportLeftOuterJoinModel> outerJoinList = null;
            IQueryable<MovieReportInnerJoinModel> innerJoinQuery = null;
            IQueryable<MovieReportLeftOuterJoinModel> outerJoinQuery = null;
            int onlyMatchedValue = viewModel.OnlyMatchedValue;
            if (onlyMatchedValue == 1)
            {
                innerJoinQuery = movieReportService.GetInnerJoinQuery();
                if (!string.IsNullOrWhiteSpace(viewModel.MovieName))
                    innerJoinQuery = innerJoinQuery.Where(m => m.MovieName.ToUpper().Contains(viewModel.MovieName.ToUpper().Trim()));
                if (viewModel.ProductionYear != null)
                    innerJoinQuery = innerJoinQuery.Where(m => m.MovieProductionYear == viewModel.ProductionYear.Value.ToString());
                if (!string.IsNullOrWhiteSpace(viewModel.BoxOfficeReturn1))
                {
                    double boxOfficeReturn1;
                    if (double.TryParse(viewModel.BoxOfficeReturn1.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn1))
                        innerJoinQuery = innerJoinQuery.Where(m => m.MovieBoxOfficeReturnValue >= boxOfficeReturn1);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.BoxOfficeReturn2))
                {
                    double boxOfficeReturn2;
                    if (double.TryParse(viewModel.BoxOfficeReturn2.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn2))
                        innerJoinQuery = innerJoinQuery.Where(m => m.MovieBoxOfficeReturnValue <= boxOfficeReturn2);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.ReviewDate1))
                {
                    DateTime reviewDate1 = DateTime.Parse(viewModel.ReviewDate1, new CultureInfo("en"));
                    innerJoinQuery = innerJoinQuery.Where(m => m.ReviewDateValue >= reviewDate1);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.ReviewDate2))
                {
                    DateTime reviewDate2 = DateTime.Parse(viewModel.ReviewDate2, new CultureInfo("en"));
                    innerJoinQuery = innerJoinQuery.Where(m => m.ReviewDateValue <= reviewDate2);
                }

                innerJoinList = innerJoinQuery.ToList();
            }
            else
            {
                outerJoinQuery = movieReportService.GetLeftOuterJoinQuery();
                if (!string.IsNullOrWhiteSpace(viewModel.MovieName))
                    outerJoinQuery = outerJoinQuery.Where(m => m.MovieName.ToUpper().Contains(viewModel.MovieName.ToUpper().Trim()));
                if (viewModel.ProductionYear != null)
                    outerJoinQuery = outerJoinQuery.Where(m => m.MovieProductionYear == viewModel.ProductionYear.Value.ToString());
                if (!string.IsNullOrWhiteSpace(viewModel.BoxOfficeReturn1))
                {
                    double boxOfficeReturn1;
                    if (double.TryParse(viewModel.BoxOfficeReturn1.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn1))
                        outerJoinQuery = outerJoinQuery.Where(m => m.MovieBoxOfficeReturnValue >= boxOfficeReturn1);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.BoxOfficeReturn2))
                {
                    double boxOfficeReturn2;
                    if (double.TryParse(viewModel.BoxOfficeReturn2.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn2))
                        outerJoinQuery = outerJoinQuery.Where(m => m.MovieBoxOfficeReturnValue <= boxOfficeReturn2);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.ReviewDate1))
                {
                    DateTime reviewDate1 = DateTime.Parse(viewModel.ReviewDate1, new CultureInfo("en"));
                    outerJoinQuery = outerJoinQuery.Where(m => m.ReviewDateValue >= reviewDate1);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.ReviewDate2))
                {
                    DateTime reviewDate2 = DateTime.Parse(viewModel.ReviewDate2, new CultureInfo("en"));
                    outerJoinQuery = outerJoinQuery.Where(m => m.ReviewDateValue <= reviewDate2);
                }

                outerJoinList = outerJoinQuery.ToList();
            }

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

            List<SelectListItem> productionYearSelectListItems = new List<SelectListItem>();
            for (int i = DateTime.Now.Year + 1; i >= 1930; i--)
            {
                productionYearSelectListItems.Add(new SelectListItem()
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }

            //return View(innerJoinModel);
            MovieReportViewModel model = new MovieReportViewModel()
            {
                InnerJoinList = innerJoinList,
                OuterJoinList = outerJoinList,
                OnlyMatchedSelectList = new SelectList(onlyMatchedSelectListItems, "Value", "Text"),
                ProductionYears = new SelectList(productionYearSelectListItems, "Value", "Text")
            };

            if (Session["MoviesReport"] != null)
                Session.Remove("MoviesReport");
            Session["MoviesReport"] = model;

            return View(model);
        }

        [HttpPost]
        public ActionResult IndexAjax(MovieReportViewModel viewModel)
        {

            List<MovieReportInnerJoinModel> innerJoinList = null;
            List<MovieReportLeftOuterJoinModel> outerJoinList = null;
            IQueryable<MovieReportInnerJoinModel> innerJoinQuery = null;
            IQueryable<MovieReportLeftOuterJoinModel> outerJoinQuery = null;
            int onlyMatchedValue = viewModel.OnlyMatchedValue;
            if (onlyMatchedValue == 1)
            {
                innerJoinQuery = movieReportService.GetInnerJoinQuery();
                if (!string.IsNullOrWhiteSpace(viewModel.MovieName))
                    innerJoinQuery = innerJoinQuery.Where(m => m.MovieName.ToUpper().Contains(viewModel.MovieName.ToUpper().Trim()));
                if (viewModel.ProductionYear != null)
                    innerJoinQuery = innerJoinQuery.Where(m => m.MovieProductionYear == viewModel.ProductionYear.Value.ToString());
                if (!string.IsNullOrWhiteSpace(viewModel.BoxOfficeReturn1))
                {
                    double boxOfficeReturn1;
                    if (double.TryParse(viewModel.BoxOfficeReturn1.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn1))
                        innerJoinQuery = innerJoinQuery.Where(m => m.MovieBoxOfficeReturnValue >= boxOfficeReturn1);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.BoxOfficeReturn2))
                {
                    double boxOfficeReturn2;
                    if (double.TryParse(viewModel.BoxOfficeReturn2.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn2))
                        innerJoinQuery = innerJoinQuery.Where(m => m.MovieBoxOfficeReturnValue <= boxOfficeReturn2);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.ReviewDate1))
                {
                    DateTime reviewDate1 = DateTime.Parse(viewModel.ReviewDate1, new CultureInfo("en"));
                    innerJoinQuery = innerJoinQuery.Where(m => m.ReviewDateValue >= reviewDate1);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.ReviewDate2))
                {
                    DateTime reviewDate2 = DateTime.Parse(viewModel.ReviewDate2, new CultureInfo("en"));
                    innerJoinQuery = innerJoinQuery.Where(m => m.ReviewDateValue <= reviewDate2);
                }

                innerJoinList = innerJoinQuery.ToList();
            }
            else
            {
                outerJoinQuery = movieReportService.GetLeftOuterJoinQuery();
                if (!string.IsNullOrWhiteSpace(viewModel.MovieName))
                    outerJoinQuery = outerJoinQuery.Where(m => m.MovieName.ToUpper().Contains(viewModel.MovieName.ToUpper().Trim()));
                if (viewModel.ProductionYear != null)
                    outerJoinQuery = outerJoinQuery.Where(m => m.MovieProductionYear == viewModel.ProductionYear.Value.ToString());
                if (!string.IsNullOrWhiteSpace(viewModel.BoxOfficeReturn1))
                {
                    double boxOfficeReturn1;
                    if (double.TryParse(viewModel.BoxOfficeReturn1.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn1))
                        outerJoinQuery = outerJoinQuery.Where(m => m.MovieBoxOfficeReturnValue >= boxOfficeReturn1);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.BoxOfficeReturn2))
                {
                    double boxOfficeReturn2;
                    if (double.TryParse(viewModel.BoxOfficeReturn2.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn2))
                        outerJoinQuery = outerJoinQuery.Where(m => m.MovieBoxOfficeReturnValue <= boxOfficeReturn2);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.ReviewDate1))
                {
                    DateTime reviewDate1 = DateTime.Parse(viewModel.ReviewDate1, new CultureInfo("en"));
                    outerJoinQuery = outerJoinQuery.Where(m => m.ReviewDateValue >= reviewDate1);
                }
                if (!string.IsNullOrWhiteSpace(viewModel.ReviewDate2))
                {
                    DateTime reviewDate2 = DateTime.Parse(viewModel.ReviewDate2, new CultureInfo("en"));
                    outerJoinQuery = outerJoinQuery.Where(m => m.ReviewDateValue <= reviewDate2);
                }

                outerJoinList = outerJoinQuery.ToList();
            }
            MovieReportViewModel model = new MovieReportViewModel()
            {
                InnerJoinList = innerJoinList,
                OuterJoinList = outerJoinList
            };
            return PartialView("_MoviesReport", model);
        }
    }
}