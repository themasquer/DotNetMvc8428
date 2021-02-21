using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DotNetMvc.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }

        [StringLength(1000, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string Content { get; set; }

        public int Rating { get; set; }

        [StringLength(200, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string Reviewer { get; set; }

        [Required(ErrorMessage = "{0} must not be empty.")]
        [DisplayName("Movie")]
        public int MovieId { get; set; }

        public MovieModel Movie { get; set; }

        [Required(ErrorMessage = "{0} must not be empty.")]
        public DateTime Date { get; set; }

        [DisplayName("Date")] 
        public string DateText => Date.ToString("yyyy/MM/dd", new CultureInfo("en"));

        public List<int> AllRatings { get; set; }
    }
}