using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DotNetMvc.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        [StringLength(200)]
        public string Reviewer { get; set; }

        public int MovieId { get; set; }
        public MovieModel Movie { get; set; }

        public DateTime Date { get; set; }

        [DisplayName("Date")] 
        public string DateText => Date.ToString("yyyy/MM/dd", new CultureInfo("en"));
    }
}