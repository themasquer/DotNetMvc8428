using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetMvc.Contexts;
using DotNetMvc.Models;
using DotNetMvc.Services;
using OfficeOpenXml;

namespace DotNetMvc.Controllers
{
    //[Authorize(Roles = "Admin")]
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

        public ActionResult Export()
        {
            MovieReportViewModel model = new MovieReportViewModel();
            MovieReportViewModel sessionModel;
            if (Session["MoviesReport"] != null)
            {
                sessionModel = Session["MoviesReport"] as MovieReportViewModel;
                model.InnerJoinList = sessionModel.InnerJoinList;
                model.OuterJoinList = sessionModel.OuterJoinList;
            }
            return View(model);
        }

        public void DownloadExcel()
        {
            MovieReportViewModel model = new MovieReportViewModel();
            MovieReportViewModel sessionModel;
            if (Session["MoviesReport"] != null)
            {
                sessionModel = Session["MoviesReport"] as MovieReportViewModel;
                model.InnerJoinList = sessionModel.InnerJoinList;
                model.OuterJoinList = sessionModel.OuterJoinList;
                if (model.InnerJoinList != null && model.InnerJoinList.Count > 0)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage();
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Movies Report");
                    excelWorksheet.Cells["A1"].Value = "Movie Name";
                    excelWorksheet.Cells["B1"].Value = "Movie Production Year";
                    excelWorksheet.Cells["C1"].Value = "Movie Box Office Return";
                    excelWorksheet.Cells["D1"].Value = "Director Name";
                    excelWorksheet.Cells["E1"].Value = "Is Director Retired?";
                    excelWorksheet.Cells["F1"].Value = "Review Content";
                    excelWorksheet.Cells["G1"].Value = "Review Rating";
                    excelWorksheet.Cells["H1"].Value = "Reviewer";
                    excelWorksheet.Cells["I1"].Value = "Review Date";
                    int row = 2;
                    foreach (var item in model.InnerJoinList)
                    {
                        excelWorksheet.Cells[string.Format("A{0}", row)].Value = item.MovieName;
                        excelWorksheet.Cells[string.Format("B{0}", row)].Value = item.MovieProductionYear;
                        excelWorksheet.Cells[string.Format("C{0}", row)].Value = item.MovieBoxOfficeReturn;
                        excelWorksheet.Cells[string.Format("D{0}", row)].Value = item.DirectorFullName;
                        excelWorksheet.Cells[string.Format("E{0}", row)].Value = item.DirectorRetired;
                        excelWorksheet.Cells[string.Format("F{0}", row)].Value = item.ReviewContent;
                        excelWorksheet.Cells[string.Format("G{0}", row)].Value = item.ReviewRating;
                        excelWorksheet.Cells[string.Format("H{0}", row)].Value = item.ReviewReviewer;
                        excelWorksheet.Cells[string.Format("I{0}", row)].Value = item.ReviewDate;
                        row++;
                    }
                    excelWorksheet.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment: filename=MoviesReport.xlsx");
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.End();
                }
                else if (model.OuterJoinList != null && model.OuterJoinList.Count > 0)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage();
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Movies Report");
                    excelWorksheet.Cells["A1"].Value = "Movie Name";
                    excelWorksheet.Cells["B1"].Value = "Movie Production Year";
                    excelWorksheet.Cells["C1"].Value = "Movie Box Office Return";
                    excelWorksheet.Cells["D1"].Value = "Director Name";
                    excelWorksheet.Cells["E1"].Value = "Is Director Retired?";
                    excelWorksheet.Cells["F1"].Value = "Review Content";
                    excelWorksheet.Cells["G1"].Value = "Review Rating";
                    excelWorksheet.Cells["H1"].Value = "Reviewer";
                    excelWorksheet.Cells["I1"].Value = "Review Date";
                    int row = 2;
                    foreach (var item in model.OuterJoinList)
                    {
                        excelWorksheet.Cells[string.Format("A{0}", row)].Value = item.MovieName;
                        excelWorksheet.Cells[string.Format("B{0}", row)].Value = item.MovieProductionYear;
                        excelWorksheet.Cells[string.Format("C{0}", row)].Value = item.MovieBoxOfficeReturn;
                        excelWorksheet.Cells[string.Format("D{0}", row)].Value = item.DirectorFullName;
                        excelWorksheet.Cells[string.Format("E{0}", row)].Value = item.DirectorRetired;
                        excelWorksheet.Cells[string.Format("F{0}", row)].Value = item.ReviewContent;
                        excelWorksheet.Cells[string.Format("G{0}", row)].Value = item.ReviewRating;
                        excelWorksheet.Cells[string.Format("H{0}", row)].Value = item.ReviewReviewer;
                        excelWorksheet.Cells[string.Format("I{0}", row)].Value = item.ReviewDate;
                        row++;
                    }
                    excelWorksheet.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment: filename=MoviesReport.xlsx");
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.End();
                }
            }
        }

        public ActionResult Json()
        {
            var model = movieReportService.GetLeftOuterJoinQuery().ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
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