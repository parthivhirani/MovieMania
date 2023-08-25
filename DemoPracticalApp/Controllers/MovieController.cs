using DemoPracticalApp.BlobStorage;
using DemoPracticalApp.Models;
using DemoPracticalApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System.Net;
using System.Net.Http.Headers;

namespace DemoPracticalApp.Controllers
{
    public class MovieController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly MovieBannerBlob _movieBannerBlob;

        public MovieController(HttpClient httpClient,
                                Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
                                MovieBannerBlob movieBannerBlob)
        {
            _httpClient = httpClient;
            _hostingEnvironment = hostingEnvironment;
            _movieBannerBlob = movieBannerBlob;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                //var result = await _httpClient.GetFromJsonAsync<List<Movie>>("https://localhost:7293/api/1.0/Movie/GetAll");
                var result = await _httpClient.GetFromJsonAsync<List<Movie>>("https://moviemaniaapi.azurewebsites.net/api/1.0/Movie/GetAll");

                if (result != null)
                {                    
                    return View(result);
                }
                ViewBag.Message = "There is no any Movies!";
                return View();
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex).ToString();
                return View("../Shared/CustomError");
            }
        }

        public async Task<IActionResult> Details(string itemid)
        {
            try
            {
                if (itemid != null)
                {
                    //var result = await _httpClient.GetFromJsonAsync<Movie>($"https://localhost:7293/api/1.0/Movie/GetByName?name={itemid}");
                    var result = await _httpClient.GetFromJsonAsync<Movie>($"https://moviemaniaapi.azurewebsites.net/api/1/Movie/GetByName?name={itemid}");
                    if (result != null)
                    {
                        return View(result);
                    }
                    return View("../Shared/CustomError");
                }
                else
                {
                    return View("../Shared/CustomError");
                }
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex).ToString();
                return View("../Shared/CustomError");
            }
            
        }

        public async Task<IActionResult> Edit(string itemid)
        {
            try
            {
                if (itemid != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["UserToken"].ToString());
                    //var result = await _httpClient.GetFromJsonAsync<Movie>($"https://localhost:7293/api/1.0/Movie/GetByName?name={itemid}");
                    var result = await _httpClient.GetFromJsonAsync<Movie>($"https://moviemaniaapi.azurewebsites.net/api/1.0/Movie/GetByName?name={itemid}");
                    if (result != null)
                    {
                        var editMovie = new EditViewModel()
                        {
                            MovieName = result.MovieName,
                            Year = result.Year,
                            Rating = result.Rating,
                            Length = result.Length,
                            Genre = result.Genre,
                            ExistingPhotoPath = result.PhotoPath
                        };

                        return View(editMovie);
                    }
                    return View("../Shared/CustomError");
                }
                else
                {
                    return View("../Shared/CustomError");
                }
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex).ToString();
                return View("../Shared/CustomError");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string itemid, EditViewModel movie)
        {
            try
            {
                if (itemid != null && ModelState.IsValid)
                {
                    string filePath = movie.ExistingPhotoPath;
                    if (movie.Photo != null)
                    {
                        filePath = await _movieBannerBlob.storeAsBlob(movie.Photo);
                    }
                    var editMovie = new Movie()
                    {
                        MovieName = movie.MovieName,
                        Year = movie.Year,
                        Rating = movie.Rating,
                        Length = movie.Length,
                        Genre = movie.Genre,
                        PhotoPath = filePath
                    };
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["UserToken"].ToString());
                    //var result = await _httpClient.PutAsJsonAsync($"https://localhost:7293/api/1.0/Movie/EditMovie?name={itemid}", editMovie);
                    var result = await _httpClient.PutAsJsonAsync($"https://moviemaniaapi.azurewebsites.net/api/1.0/Movie/EditMovie?name={itemid}", editMovie);
                    if (result.StatusCode == (HttpStatusCode)200)
                    {
                        return Redirect($"/Movie/Details?itemid={movie.MovieName}");
                    }
                    ViewBag.Error = "Movie can't be updated!";
                    return View(movie);
                }
                else
                {
                    ViewBag.Error = "Please enter valid movie details.";
                    return View(movie);
                }
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex).ToString();
                return View("../Shared/CustomError");
            }
            
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["UserToken"].ToString());
                return View();
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex).ToString();
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieViewModel movieViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string filePath = null;
                    if (movieViewModel.Photo != null)
                    {
                        filePath = await _movieBannerBlob.storeAsBlob(movieViewModel.Photo);
                    }

                    Movie newMovie = new Movie()
                    {
                        MovieName = movieViewModel.MovieName,
                        Year = movieViewModel.Year,
                        Rating = movieViewModel.Rating,
                        Length = movieViewModel.Length,
                        Genre = movieViewModel.Genre,
                        PhotoPath = filePath,
                    };
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["UserToken"].ToString());
                    //var result = await _httpClient.PostAsJsonAsync("https://localhost:7293/api/1.0/Movie/AddMovie", newMovie);
                    var result = await _httpClient.PostAsJsonAsync("https://moviemaniaapi.azurewebsites.net/api/1.0/Movie/AddMovie", newMovie);
                    if (result.StatusCode == (HttpStatusCode)200 && result != null)
                    {
                        return RedirectToAction("Index");
                    }
                    //else if(result.StatusCode == (HttpStatusCode)400)
                    //{
                    //    var respData = await result.Content.ReadAsStringAsync();
                    //    dynamic errorDetails = JsonConvert.DeserializeObject(respData);
                    //    errorDetails.errors.Year
                    //}
                    return View("../Shared/CustomError");
                }
                else
                {
                    return View("../Shared/CustomError");
                }
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex).ToString();
                return View("../Shared/CustomError");
            }
        }

        public async Task<IActionResult> Delete(string itemid)
        {
            try
            {
                if (itemid != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["UserToken"].ToString());
                    //var result = await _httpClient.GetFromJsonAsync<Movie>($"https://localhost:7293/api/1.0/Movie/GetByName?name={itemid}");
                    var result = await _httpClient.GetFromJsonAsync<Movie>($"https://moviemaniaapi.azurewebsites.net/api/1.0/Movie/GetByName?name={itemid}");
                    if (result != null)
                    {
                        return View(result);
                    }
                    return View("../Shared/CustomError");
                }
                else
                {
                    return View("../Shared/CustomError");
                }
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex).ToString();
                return View("../Shared/CustomError");
            }
            
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string itemid, string photoPath)
        {
            try
            {
                if (itemid != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["UserToken"].ToString());
                    //var result = await _httpClient.DeleteAsync($"https://localhost:7293/api/1.0/Movie/DeleteMovie?name={itemid}");
                    var result = await _httpClient.DeleteAsync($"https://moviemaniaapi.azurewebsites.net/api/1.0/Movie/DeleteMovie?name={itemid}");
                    if (result.StatusCode == (HttpStatusCode)200 && result != null)
                    {
                        //await _movieBannerBlob.deleteFromBlob(photoPath);
                        return RedirectToAction("Index", "Movie");
                    }
                    else
                    {
                        ViewBag.Error = "Movie can't be deleted!";
                        return RedirectToAction("Delete", itemid);
                    }
                }
                ViewBag.Error = "Movie is not valid.";
                return RedirectToAction("Delete", itemid);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex).ToString();
                return View("../Shared/CustomError");
            }
        }
    }
}
