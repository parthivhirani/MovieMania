using System.ComponentModel.DataAnnotations;

namespace DemoPracticalApp.ViewModels
{
    public class MovieViewModel
    {
        public string MovieName { get; set; }
        public int Year { get; set; }
        [Range(0, 10)]
        public double Rating { get; set; }
        [Range(0, 4)]
        public double Length { get; set; }
        public string Genre { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
