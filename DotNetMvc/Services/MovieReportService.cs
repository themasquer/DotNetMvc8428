using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using DotNetMvc.Contexts;
using DotNetMvc.Models;

namespace DotNetMvc.Services
{
    public class MovieReportService
    {
        private readonly MoviesContext _db;

        public MovieReportService(MoviesContext db)
        {
            _db = db;
        }

        public IQueryable<MovieReportInnerJoinModel> GetInnerJoinQuery()
        {
            var movieQuery = _db.Movies.AsQueryable();
            var movieDirectorQuery = _db.MovieDirectors.AsQueryable();
            var directorQuery = _db.Directors.AsQueryable();
            var reviewQuery = _db.Reviews.AsQueryable();
            IQueryable<MovieReportInnerJoinModel> query = from m in movieQuery
                                                          join md in movieDirectorQuery
                                                              on m.Id equals md.MovieId
                                                          join d in directorQuery
                                                              on md.DirectorId equals d.Id
                                                          join r in reviewQuery
                                                              on m.Id equals r.MovieId
                                                          select new MovieReportInnerJoinModel()
                                                          {
                                                              DirectorFullName = d.Name + " " + d.Surname,
                                                              DirectorRetiredValue = d.Retired,
                                                              MovieBoxOfficeReturnValue = m.BoxOfficeReturn,
                                                              MovieName = m.Name,
                                                              MovieProductionYear = m.ProductionYear,
                                                              ReviewContent = r.Content,
                                                              ReviewDateValue = r.Date,
                                                              ReviewRating = r.Rating,
                                                              ReviewReviewer = r.Reviewer
                                                          };
            return query;
        }

        public IQueryable<MovieReportLeftOuterJoinModel> GetLeftOuterJoinQuery()
        {
            var movieQuery = _db.Movies.AsQueryable();
            var movieDirectorQuery = _db.MovieDirectors.AsQueryable();
            var directorQuery = _db.Directors.AsQueryable();
            var reviewQuery = _db.Reviews.AsQueryable();
            IQueryable<MovieReportLeftOuterJoinModel> query = from m in movieQuery
                                                              join md in movieDirectorQuery
                                                                  on m.Id equals md.MovieId into movieDirectorJoin
                                                              from subMovieDirectorJoin in movieDirectorJoin.DefaultIfEmpty()
                                                              join d in directorQuery
                                                                  on subMovieDirectorJoin.DirectorId equals d.Id into directorJoin
                                                              from subDirectorJoin in directorJoin.DefaultIfEmpty()
                                                              join r in reviewQuery
                                                                  on m.Id equals r.MovieId into reviewJoin
                                                              from subReviewJoin in reviewJoin.DefaultIfEmpty()
                                                              select new MovieReportLeftOuterJoinModel()
                                                              {
                                                                  DirectorFullName = (subDirectorJoin.Name + " " + subDirectorJoin.Surname) ?? "",
                                                                  DirectorRetiredValue = subDirectorJoin.Retired,
                                                                  MovieBoxOfficeReturnValue = m.BoxOfficeReturn,
                                                                  MovieName = m.Name,
                                                                  MovieProductionYear = m.ProductionYear,
                                                                  ReviewContent = subReviewJoin.Content ?? "",
                                                                  ReviewDateValue = subReviewJoin.Date,
                                                                  ReviewRating = subReviewJoin.Rating,
                                                                  ReviewReviewer = subReviewJoin.Reviewer ?? ""
                                                              };
            return query;
        }
    }
}