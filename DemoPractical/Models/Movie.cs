using System.ComponentModel.DataAnnotations;

namespace DemoPractical.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string MovieName { get; set;}
        public int Year { get; set; }
        
        public double Rating { get; set; }
        
        public double Length { get; set; }
        public string Genre { get; set; }

        public string PhotoPath { get; set; }
    }
}
