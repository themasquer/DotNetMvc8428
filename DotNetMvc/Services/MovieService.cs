using System;
using System.Data.Entity;
using DotNetMvc.Contexts;
using DotNetMvc.Models;
using System.Linq;
using DotNetMvc.Entities;

namespace DotNetMvc.Services
{
    public class MovieService
    {
        private readonly MoviesContext _db;

        public MovieService(MoviesContext db)
        {
            _db = db;
        }

        // read
        public IQueryable<MovieModel> GetQuery()
        {
            return _db.Movies.OrderBy(m => m.Name).Select(m => new MovieModel()
            {
                Id = m.Id,
                Name = m.Name,
                ProductionYear = m.ProductionYear,
                BoxOfficeReturn = m.BoxOfficeReturn,
                Directors = m.MovieDirectors.Select(md => new DirectorModel()
                {
                    Id = md.Director.Id,
                    Name = md.Director.Name,
                    Surname = md.Director.Surname,
                    Retired = md.Director.Retired
                }).ToList(),
                Reviews = m.Reviews.Select(r => new ReviewModel()
                {
                    Id = r.Id,
                    Content = r.Content,
                    MovieId = r.MovieId,
                    Rating = r.Rating,
                    Reviewer = r.Reviewer
                }).ToList()
            });
        }

        public bool Add(MovieModel model)
        {
            try
            {
                Movie entity = new Movie()
                {
                    Name = model.Name,
                    ProductionYear = model.ProductionYear,
                    BoxOfficeReturn = model.BoxOfficeReturn
                };
                _db.Movies.Add(entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public bool Update(MovieModel model)
        {
            try
            {
                Movie entity = _db.Movies.Find(model.Id);
                entity.Name = model.Name;
                entity.ProductionYear = model.ProductionYear;
                entity.BoxOfficeReturn = model.BoxOfficeReturn;
                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}