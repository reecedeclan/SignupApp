using Microsoft.AspNetCore.Mvc;
using SignupApp.Models;
using SignupApp.Services;
using System.Threading.Tasks;
using SignupApp.ViewModels;

namespace SignupApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the username is already taken
                var existingUser = await _userService.GetUserByUsernameAsync(model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View(model);
                }

                // Create a new user
                var user = new User
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Username = model.Username,
                    PasswordHash = model.Password // This will be hashed in the service
                };

                await _userService.RegisterUserAsync(user);
                return RedirectToAction("Index", "Home"); // Redirect to a success page or home
            }

            return View(model); // If model state is invalid, return the view with validation errors
        }
    }
}
