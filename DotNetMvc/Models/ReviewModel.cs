using System.ComponentModel.DataAnnotations;

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
    }
}