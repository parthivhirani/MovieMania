using System.ComponentModel.DataAnnotations;

namespace DemoPractical.Models
{
    public class MovieCast
    {
        [Key]
        public string MovieName { get; set; }
        public string Actor { get; set; }
        public string Actress { get; set; }
    }
}
