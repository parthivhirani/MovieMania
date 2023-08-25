using DemoPractical.Models;

namespace DemoPractical.Repository
{
    public interface IMovieCastRepository
    {
        MovieCast GetMovieCast(string name);
        string AddMovieCast(string name, MovieCast movieCast);
    }
}
