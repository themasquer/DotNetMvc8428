using System.Linq;
using DotNetMvc.Contexts;
using DotNetMvc.Models;

namespace DotNetMvc.Services
{
    public class ReviewService
    {
        private readonly MoviesContext _db;

        public ReviewService(MoviesContext db)
        {
            _db = db;
        }

        public IQueryable<ReviewModel> GetQuery()
        {
            return null;
        }
    }
}