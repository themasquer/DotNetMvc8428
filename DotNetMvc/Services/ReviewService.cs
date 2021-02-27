using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DotNetMvc.Contexts;
using DotNetMvc.Entities;
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

        public void Add(ReviewModel review)
        {
            try
            {
                Review entity = new Review()
                {
                    Content = review.Content,
                    Date = review.Date.Value,
                    MovieId = review.MovieId,
                    Rating = review.Rating,
                    Reviewer = string.IsNullOrWhiteSpace(review.Reviewer) ? "Anonymous" : review.Reviewer
                };
                _db.Reviews.Add(entity);
                _db.SaveChanges();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void Update(ReviewModel review)
        {
            try
            {
                Review entity = _db.Reviews.Find(review.Id);
                entity.Content = review.Content;
                entity.Date = review.Date.Value;
                entity.MovieId = review.MovieId;
                entity.Rating = review.Rating;
                entity.Reviewer = string.IsNullOrWhiteSpace(review.Reviewer) ? "Anonymous" : review.Reviewer;
                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void Delete(int id)
        {
            try
            {
                Review entity = _db.Reviews.Find(id);
                _db.Reviews.Remove(entity);
                _db.SaveChanges();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
