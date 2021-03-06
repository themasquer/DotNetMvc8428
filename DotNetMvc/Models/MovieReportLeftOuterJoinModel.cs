﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DotNetMvc.Models
{
    public class MovieReportLeftOuterJoinModel
    {
        [DisplayName("Movie Name")]
        public string MovieName { get; set; }

        [DisplayName("Movie Production Year")]
        public string MovieProductionYear { get; set; }

        [DisplayName("Movie Box Office Return")]
        public string MovieBoxOfficeReturn => MovieBoxOfficeReturnValue == null ? "" : MovieBoxOfficeReturnValue.Value.ToString(new CultureInfo("en"));

        public double? MovieBoxOfficeReturnValue { get; set; }

        [DisplayName("Director Name")]
        public string DirectorFullName { get; set; }

        [DisplayName("Is Director Retired?")]
        public string DirectorRetired => !DirectorRetiredValue.HasValue ? "" : (DirectorRetiredValue.Value ? "Yes" : "No");

        public bool? DirectorRetiredValue { get; set; }

        [DisplayName("Review Content")]
        public string ReviewContent { get; set; }

        [DisplayName("Review Rating")]
        public int? ReviewRating { get; set; }

        public string ReviewRatingText => ReviewRating == null ? "" : ReviewRating.Value.ToString();

        [DisplayName("Reviewer")]
        public string ReviewReviewer { get; set; }

        [DisplayName("Review Date")]
        public string ReviewDate => ReviewDateValue == null ? "" : ReviewDateValue.Value.ToString("MM/dd/yyyy", new CultureInfo("en"));

        public DateTime? ReviewDateValue { get; set; }
    }
}