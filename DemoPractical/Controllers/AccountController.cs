using DemoPractical.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoPractical.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signManager;
        private readonly ILogger<AccountController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<IdentityUser> signInManager,
                                UserManager<IdentityUser> userManager,
                                ILogger<AccountController> logger,
                                RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var validUser = await _userManager.FindByEmailAsync(user.Email);
                if (validUser != null && await _userManager.CheckPasswordAsync(validUser, user.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(validUser);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenOptions = new JwtSecurityToken(
                        issuer: "https://localhost:5001",
                        audience: "https://localhost:5001",
                        claims: authClaims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: signinCredentials
                    );

                    await _signManager.SignInAsync(validUser, isPersistent: false);
                    _logger.LogInformation("User logged in successfully");
                    
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(tokenOptions),
                        expiration = tokenOptions.ValidTo,
                        email = user.Email,
                        roles = userRoles
                    });
                }
                else
                {
                    _logger.LogError("User can't logged in because of invalid credentials");
                    return Unauthorized("User is not valid");
                }
            }
            else
            {
                _logger.LogError("User can't logged in because of invalid credentials");
                return Unauthorized("User is not valid");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel user)
        {
            if(ModelState.IsValid)
            {
                var newUser = new IdentityUser()
                {
                    Email = user.Email,
                    UserName = user.Email
                };

                var result = await _userManager.CreateAsync(newUser, user.Password);
                await _userManager.AddToRoleAsync(newUser, "User");
                if (!result.Succeeded)
                {
                    _logger.LogError("User can't registered because of some internal error");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                _logger.LogInformation("User registered successfully");
                return Ok("User created successfully");
            }
            else
            {
                _logger.LogError("User can't registered because of invalid details");
                return BadRequest("Please enter valid details");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            _logger.LogInformation("User logged out successfully");
            return Ok("User logged out successfully");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel updatePasswordModel)
        {
            if (ModelState.IsValid)
            {

                var user1 = await _userManager.FindByEmailAsync(updatePasswordModel.Email);
                if(user1 != null && user1.PasswordHash != updatePasswordModel.CurrentPassword)
                {
                    var user = await _userManager.CheckPasswordAsync(user1, updatePasswordModel.CurrentPassword);
                    var existingUser = await _userManager.CheckPasswordAsync(user1, updatePasswordModel.NewPassword);
                    if (user && !existingUser)
                    {
                        var result = await _userManager.ChangePasswordAsync(user1, updatePasswordModel.CurrentPassword, updatePasswordModel.NewPassword);
                        if(result.Succeeded)
                            return Ok();
                        return BadRequest();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterViewModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var user = new IdentityUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            if (await _roleManager.RoleExistsAsync("Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return Ok();
        }
    }
}
