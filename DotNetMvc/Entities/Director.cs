using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNetMvc.Entities
{
    public class Director
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        public bool Retired { get; set; }

        //public List<Movie> Movies { get; set; }
        public virtual List<MovieDirector> MovieDirectors { get; set; }
    }
}