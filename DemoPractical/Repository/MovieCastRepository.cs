using DemoPractical.Data;
using DemoPractical.Models;
using Sentry;

namespace DemoPractical.Repository
{
    public class MovieCastRepository : IMovieCastRepository
    {
        private readonly MovieDBContext _context;
        private readonly ILogger<MovieCastRepository> _logger;

        public MovieCastRepository(MovieDBContext context,
                                    ILogger<MovieCastRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public MovieCast GetMovieCast(string name)
        {
            _logger.LogInformation($"{name} movie's cast details returned");
            return _context.MoviesCasts.Where(n => n.MovieName == name).FirstOrDefault();
        }


        public string AddMovieCast(string name,  MovieCast movieCast)
        {
            try
            {
                if (_context.Movies.Where(m => m.MovieName == name).Any())
                {
                    var newMovieCast = new MovieCast()
                    {
                        MovieName = name,
                        Actor = movieCast.Actor,
                        Actress = movieCast.Actress
                    };
                    _context.MoviesCasts.Add(newMovieCast);
                    _context.SaveChanges();
                    _logger.LogInformation($"{name} movie's cast added successfully");
                    return "Movie cast added successfully";
                }
                else
                {
                    _logger.LogError($"There is no such movie with name {name}");
                    return "No such movie found!";
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return SentrySdk.CaptureException(ex).ToString();
            }
            
        }
    }
}
