using DemoPractical.Models;

namespace DemoPractical.Repository
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAllMovieDetails();
        Movie GetMovieDetailsByName(string name);
        string AddNewMovie(Movie movie);
        string UpdateMovieDetails(string name, Movie movie);
        string DeleteMovieDetails(string name);
    }
}
