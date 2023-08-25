using DemoPractical.Models;
using DemoPractical.Data;
using System.Web.Mvc;
using Sentry;

namespace DemoPractical.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDBContext _context;

        public MovieRepository(MovieDBContext movieDBContext)
        {
            _context = movieDBContext;
        }

        public string AddNewMovie(Movie movie)
        {
            try
            {
                var checkExisting = _context.Movies
                    .Where(x => x.MovieName
                    .ToLower() == movie.MovieName
                    .ToLower())
                    .SingleOrDefault();
                if(checkExisting != null)
                {
                    return "Movie already exist!";
                }
                else
                {
                    _context.Movies.Add(movie);
                    var op = _context.SaveChanges();
                    if (op > 0)
                    {
                        return "Movie added successfully.";
                    }
                    else
                    {
                        return "There is some error while entering new movie";
                    }
                }
            }
            catch(Exception ex)
            {
                return SentrySdk.CaptureException(ex).ToString();
            }
            
        }

        public string DeleteMovieDetails(string name)
        {
            var deleteMovie = _context.Movies.Where(m => m.MovieName == name).FirstOrDefault();
            if (name != null && deleteMovie != null)
            {
                _context.Movies.Remove(deleteMovie);
                _context.SaveChanges();
                return "Movie deleted successfully.";
            }
            else
            {
                return "Movie can't be deleted.";
            }
        }

        public IEnumerable<Movie> GetAllMovieDetails()
        {
            return _context.Movies.ToList();
        }

        public Movie GetMovieDetailsByName(string name)
        {
            return _context.Movies.Where(m => m.MovieName == name).FirstOrDefault();
        }

        public string UpdateMovieDetails(string name, Movie movie)
        {
            var editMovie = _context.Movies.Where(m => m.MovieName == name).FirstOrDefault();
            if (name != null && editMovie != null)
            {
                editMovie.MovieName = movie.MovieName;
                editMovie.Year = movie.Year;
                editMovie.Rating = movie.Rating;
                editMovie.Length = movie.Length;
                editMovie.Genre = movie.Genre;
                editMovie.PhotoPath = movie.PhotoPath;
                _context.SaveChanges();
                return "Movie edited successfully.";
            }
            else
            {
                return "Movie is not available.";
            }
        }
    }
}
