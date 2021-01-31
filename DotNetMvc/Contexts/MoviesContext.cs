using System.Data.Entity;
using DotNetMvc.Entities;

namespace DotNetMvc.Contexts
{
    public class MoviesContext : DbContext
    {
        public MoviesContext() : base("MoviesContext")
        {
            
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MovieDirector> MovieDirectors { get; set; }
    }
}