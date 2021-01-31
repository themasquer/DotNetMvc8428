﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNetMvc.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(4)]
        public string ProductionYear { get; set; }

        public double? BoxOfficeReturn { get; set; }

        //public List<Director> Directors { get; set; }
        public virtual List<MovieDirector> MovieDirectors { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}