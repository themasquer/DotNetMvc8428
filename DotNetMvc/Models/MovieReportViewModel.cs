using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetMvc.Models
{
    public class MovieReportViewModel
    {
        public List<MovieReportInnerJoinModel> InnerJoinList { get; set; }
        public List<MovieReportLeftOuterJoinModel> OuterJoinList { get; set; }

        public SelectList OnlyMatchedSelectList { get; set; }

        [DisplayName("Only Matched")] 
        public int OnlyMatchedValue { get; set; } = 1;

        [DisplayName("Movie Name")]
        public string MovieName { get; set; }

        [DisplayName("Production Year")]
        public int? ProductionYear { get; set; }

        public SelectList ProductionYears { get; set; }

        [DisplayName("Box Office Return")]
        public string BoxOfficeReturn1 { get; set; }

        public string BoxOfficeReturn2 { get; set; }

        [DisplayName("Review Date")]
        public string ReviewDate1 { get; set; }

        public string ReviewDate2 { get; set; }
    }
}