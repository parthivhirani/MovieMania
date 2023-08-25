using DemoPractical.Controllers;
using DemoPractical.Models;
using DemoPractical.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace MoqTestingProject
{
    public class MovieTest
    {
        private readonly Mock<IMovieRepository> _mock;
        public MovieTest()
        {
            _mock = new Mock<IMovieRepository>();
        }

        [Fact]
        public void GetMovieByName()
        {
            // arrange
            var expectedData = new Movie()
            {
                MovieId = 3,
                MovieName = "3 Idiots",
                Year = 2009,
                Rating = 8.4,
                Length = 2.5,
                Genre = "Comedy",
                PhotoPath = "ce61834e-1d86-453a-b67a-2ff2b6f59098_3idiots.png"
            };
            _mock.Setup(m => m.GetMovieDetailsByName("3 Idiots")).Returns(expectedData);
            var movieController = new MovieController(_mock.Object, new Logger<MovieController>(new LoggerFactory()));

            // act
            var actual = movieController.GetByName("3 Idiots") as OkObjectResult;
            var actualObj = JsonConvert.SerializeObject(actual.Value);
            var actualData = JsonConvert.DeserializeObject<Movie>(actualObj);

            // assert
            Assert.NotNull(actual);
            Assert.True((actualData.MovieId).Equals(expectedData.MovieId));
        }
    }
}