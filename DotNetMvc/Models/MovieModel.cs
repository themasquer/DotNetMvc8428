using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNetMvc.Models
{
    public class MovieModel
    {
        //todo: Validation özelleştirme
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(4)]
        public string ProductionYear { get; set; }

        public double? BoxOfficeReturn { get; set; }

        public List<DirectorModel> Directors { get; set; }
        public List<ReviewModel> Reviews { get; set; }
    }
}