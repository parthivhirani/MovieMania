using DemoPractical.Models;
using DemoPractical.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace DemoPractical.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/{v:apiVersion}/[controller]/[action]")]
    public class MovieCastController : Controller
    {
        private readonly IMovieCastRepository _movieCastRepository;

        public MovieCastController(IMovieCastRepository movieCastRepository)
        {
            _movieCastRepository = movieCastRepository;
        }

        [HttpGet]
        public MovieCast GetCastByMovieName(string name)
        {
            return _movieCastRepository.GetMovieCast(name);
        }

        [Authorize]
        [HttpPost]
        public string AddMovieCastDetails(string name, MovieCast movieCast)
        {
            return _movieCastRepository.AddMovieCast(name, movieCast);
        }
    }
}
