using System.Collections.Generic;
using System.Linq;
using DotNetMvc.Contexts;
using DotNetMvc.Models;

namespace DotNetMvc.Services
{
    public class ReviewService
    {
        // AutoMapper: https://automapper.org/

        private readonly MoviesContext _db;

        public ReviewService(MoviesContext db)
        {
            _db = db;
        }

        public IQueryable<ReviewModel> GetQuery()
        {
            return _db.Reviews.OrderByDescending(r => r.Date).Select(r => new ReviewModel()
            {
                Id = r.Id,
                Content = r.Content,
                Date = r.Date,
                Reviewer = r.Reviewer,
                Rating = r.Rating,
                MovieId = r.MovieId,
                Movie = r.Movie != null ? new MovieModel()
                {
                    Id = r.Movie.Id,
                    Name = r.Movie.Name,
                    BoxOfficeReturn = r.Movie.BoxOfficeReturn,
                    ProductionYear = r.Movie.ProductionYear,
                    Directors = r.Movie.MovieDirectors.Select(md => new DirectorModel()
                    {
                        Id = md.Director.Id,
                        Name = md.Director.Name,
                        Surname = md.Director.Surname,
                        Retired = md.Director.Retired
                    }).ToList()
                } : null
            });
        }

        public void FillAllRatings(ReviewModel review)
        {
            review.AllRatings = new List<int>();
            for (int i = 1; i <= 10; i++)
            {
                review.AllRatings.Add(i);
            }
        }
    }
}
