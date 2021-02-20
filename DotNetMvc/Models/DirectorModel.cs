using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DotNetMvc.Models
{
    public class DirectorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} must not be emtpy.")]
        //[StringLength(100, ErrorMessage = "{0} must be maximum {1} characters.")]
        [MinLength(2, ErrorMessage = "{0} must be minimum {1} characters.")]
        [MaxLength(100, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} must not be emtpy.")]
        //[StringLength(100, ErrorMessage = "{0} must be maximum {1} characters.")]
        [MinLength(2, ErrorMessage = "{0} must be minimum {1} characters.")]
        [MaxLength(100, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string Surname { get; set; }

        public bool Retired { get; set; }

        [DisplayName("Retired")]
        // 1
        //public string RetiredText { get; set; }

        // 2
        //public string RetiredText
        //{
        //    get { return Retired ? "Yes" : "No"; } 
        //}
        public string RetiredText => Retired ? "Yes" : "No";

        public List<MovieModel> Movies { get; set; }

        [DisplayName("Movies")]
        public string MoviesText => Movies == null || Movies.Count == 0 ? "" : string.Join("<br />", Movies.Select(m => m.Name));

        [DisplayName("Movies")]
        [Required(ErrorMessage = "At least one movie must be selected.")]
        public List<int> MovieIds { get; set; }

        private string _fullName;
        public string FullName
        {
            get
            {
                _fullName = Name + " " + Surname;
                return _fullName;
            }
        }
    }
}