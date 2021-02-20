using System;
using System.Data.Entity;
using System.Linq;
using DotNetMvc.Contexts;
using DotNetMvc.Entities;
using DotNetMvc.Models;

namespace DotNetMvc.Services
{
    public class DirectorService
    {
        private readonly MoviesContext _db;

        public DirectorService(MoviesContext db)
        {
            _db = db;
        }

        public IQueryable<DirectorModel> GetQuery()
        {
            return _db.Directors.OrderBy(d => d.Name).ThenBy(d => d.Surname).Select(d => new DirectorModel()
            {
                Id = d.Id,
                Name = d.Name,
                Surname = d.Surname,
                Retired = d.Retired,

                // 1
                //RetiredText = d.Retired ? "Yes" : "No",
                // 2

                Movies = d.MovieDirectors.Select(md => new MovieModel()
                {
                    Id = md.Movie.Id,
                    Name = md.Movie.Name,
                    BoxOfficeReturn = md.Movie.BoxOfficeReturn,
                    ProductionYear = md.Movie.ProductionYear
                }).ToList(),

                MovieIds = d.MovieDirectors.Select(md => md.MovieId).ToList()
            });
        }

        public bool Add(DirectorModel model)
        {
            try
            {
                Director entity = new Director()
                {
                    Name = model.Name.Trim(),
                    Surname = model.Surname.Trim(),
                    Retired = model.Retired,
                    MovieDirectors = model.MovieIds.Select(mId => new MovieDirector()
                    {
                        MovieId = mId
                    }).ToList()
                };
                _db.Directors.Add(entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public bool Update(DirectorModel model)
        {
            try
            {
                Director entity = _db.Directors.Find(model.Id);
                _db.MovieDirectors.RemoveRange(entity.MovieDirectors);
                entity.Name = model.Name.Trim();
                entity.Surname = model.Surname.Trim();
                entity.Retired = model.Retired;
                entity.MovieDirectors = model.MovieIds.Select(mId => new MovieDirector()
                {
                    MovieId = mId
                }).ToList();
                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                Director entity = _db.Directors.Find(id);
                _db.MovieDirectors.RemoveRange(entity.MovieDirectors);
                _db.Directors.Remove(entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
    }
}