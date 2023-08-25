using DemoPracticalApp.Models;
using DemoPracticalApp.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace DemoPracticalApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public AccountController(HttpClient httpClient,
                                IHttpContextAccessor httpContextAccessor,
                                IDataProtectionProvider dataProtectionProvider)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _dataProtectionProvider = dataProtectionProvider;
        }


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(ModelState.IsValid)
            {
                //var result = await _httpClient.PostAsJsonAsync("https://localhost:7293/api/Account/Login", loginViewModel);
                var result = await _httpClient.PostAsJsonAsync("https://moviemaniaapi.azurewebsites.net/api/Account/Login", loginViewModel);

                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<AuthenticationResponse>(data);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Token.ToString());
                    CookieOptions options = new CookieOptions();
                    options.Expires = DateTime.Now.AddMinutes(60);

                    var roleProtector = _dataProtectionProvider.CreateProtector("RoleProtector");
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("UserRole", roleProtector.Protect(response.Roles[0]), options);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("UserToken", response.Token, options);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("UserEmail", response.Email, options);
                    return RedirectToAction("Index", "Movie");
                }
                ViewBag.Error = "Invalid credentials";
                return View(loginViewModel);
            }
            else
            {
                ViewBag.Error = "Please enter valid details";
                return View(loginViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                //var result = await _httpClient.PostAsJsonAsync("https://localhost:7293/api/Account/Register", registerViewModel);
                var result = await _httpClient.PostAsJsonAsync("https://moviemaniaapi.azurewebsites.net/api/Account/Register", registerViewModel);
                var data = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Login");
                }
                ViewBag.Error = "Invalid user details";
                return View(registerViewModel);
            }
            else
            {
                ViewBag.Error = "Please enter valid details";
                return View(registerViewModel);
            }
        }

        public async Task<IActionResult> Logout()
        {
            //await _httpClient.PostAsync("https://localhost:7293/api/Account/Logout", null);
            await _httpClient.PostAsync("https://moviemaniaapi.azurewebsites.net/api/Account/Logout", null);
            Response.Cookies.Delete("UserToken");
            Response.Cookies.Delete("UserEmail");
            Response.Cookies.Delete("UserRole");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel updatePasswordModel)
        {
            var email = Request.Cookies["UserEmail"].ToString();
            if (email != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["UserToken"].ToString());
                updatePasswordModel.Email = email;
                //var response = await _httpClient.PostAsJsonAsync("https://localhost:7293/api/Account/UpdatePassword", updatePasswordModel);
                var response = await _httpClient.PostAsJsonAsync("https://moviemaniaapi.azurewebsites.net/api/Account/UpdatePassword", updatePasswordModel);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Response.Cookies.Delete("UserToken");
                    Response.Cookies.Delete("UserEmail");
                    Response.Cookies.Delete("UserRole");
                    ViewBag.Success = "Password updated successfully";
                    return View("Login");
                }
                else
                {
                    ViewBag.Error = "Password can't be updated";
                    return View(updatePasswordModel);
                }
            }
            else
            {
                ViewBag.Error = "Password can't be updated";
                return View(updatePasswordModel);
            }
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
