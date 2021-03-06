﻿using System;
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

        [Required(ErrorMessage = "{0} must be selected.")]
        [DisplayName("Movie")]
        public int MovieId { get; set; }

        public MovieModel Movie { get; set; }

        [Required(ErrorMessage = "{0} must not be empty.")]
        public DateTime? Date { get; set; }

        [DisplayName("Date")] 
        public string DateText => Date.HasValue ? Date.Value.ToString("yyyy/MM/dd", new CultureInfo("en")) : "";

        [DisplayName("Date")]
        public string DateValue
        {
            get
            {
                //return Date.HasValue ? Date.Value.ToString("dd.MM.yyyy", new CultureInfo("tr")) : "";
                return Date.HasValue ? Date.Value.ToString("MM/dd/yyyy", new CultureInfo("en")) : "";
            }
            set
            {
                Date = null;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    //Date = DateTime.Parse(value, new CultureInfo("tr"));
                    Date = DateTime.Parse(value, new CultureInfo("en"));
                }
            }
        }

        public List<int> AllRatings { get; set; }
    }
}