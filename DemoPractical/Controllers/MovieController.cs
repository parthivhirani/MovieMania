using DemoPractical.Models;
using DemoPractical.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Net.Http.Headers;

namespace DemoPractical.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]/[action]")]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<MovieController> _logger;

        public MovieController(IMovieRepository movieRepository,
                                ILogger<MovieController> logger)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var movies = _movieRepository.GetAllMovieDetails();
            _logger.LogInformation("User retrieved all movies details");
            if(movies!=null)
                return Ok(movies.ToList());
            return NotFound();
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        [OutputCache(Duration = 5)]
        public IActionResult GetByName(string name)
        {
            var movieDetails = _movieRepository.GetMovieDetailsByName(name);
            _logger.LogInformation($"User retrieved {name} movie's details");
            if(movieDetails!=null)
                return Ok(movieDetails);
            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddMovie(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _movieRepository.AddNewMovie(movie);
                _logger.LogInformation("New Movie added successfully");
                return Ok("Movie added successfully");
            }
            else
            {
                _logger.LogError("Movie can't be added due to invalid details");
                return BadRequest("Please enter valid movie details");
            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult EditMovie(string name, Movie movie)
        {
            if(ModelState.IsValid)
            {
                var status = _movieRepository.UpdateMovieDetails(name, movie);
                _logger.LogInformation("Movie edited successfully");
                return Ok(status);
            }
            else
            {
                _logger.LogError("Movie can't be edited due to invalid movie details");
                return BadRequest("Please enter valid movie details");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public IActionResult DeleteMovie(string name)
        {
            if (ModelState.IsValid)
            {
                var status = _movieRepository.DeleteMovieDetails(name);
                _logger.LogInformation("Movie deleted successfully");
                return Ok(status);
            }
            else
            {
                _logger.LogError("Movie can't be deleted due to invalid movie name");
                return NotFound("Please enter valid movie name");
            }
         }
    }
}
