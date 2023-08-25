using System.ComponentModel.DataAnnotations;

namespace DemoPracticalApp.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public int Year { get; set; }
        [Range(0, 10)]
        public double Rating { get; set; }
        [Range(0, 4)]
        public double Length { get; set; }
        public string Genre { get; set; }

        public string? PhotoPath { get; set; }
    }
}
