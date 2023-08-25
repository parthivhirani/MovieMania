using DemoPractical.Data;
using DemoPractical.Models;
using FluentValidation;

namespace DemoPractical.Validations
{
    public class MovieValidator: AbstractValidator<Movie>
    {
        public MovieValidator()
        {
            RuleFor(m => m.MovieName).NotEmpty().MaximumLength(100).WithMessage("Exceeding movie name length"); //.Must(UniqueName).WithMessage("This Movie name already exist!");
            RuleFor(m => m.Year).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Invalid year of movie");
            RuleFor(m => m.Rating).NotEmpty().InclusiveBetween(0, 10).WithMessage("Rating can't be more than 10");
            RuleFor(m => m.Length).NotEmpty().InclusiveBetween(0, 4).WithMessage("Movie length can't more than 4 hour");
            RuleFor(m => m.Genre).NotEmpty();
        }

        //private bool UniqueName(string name)
        //{
        //    var _db = new MovieDBContext();
        //    var dbCategory = _db.Movies
        //                        .Where(x => x.MovieName.ToLower() == name.ToLower())
        //                        .SingleOrDefault();

        //    if (dbCategory == null)
        //        return true;
        //    return false;
        //}
    }
}
