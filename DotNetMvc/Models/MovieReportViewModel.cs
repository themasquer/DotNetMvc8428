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
    }
}