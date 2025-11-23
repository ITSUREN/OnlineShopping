using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Models.Domain;
using OnlineShopping.Models.ViewModels;
using OnlineShopping.Services.Interfaces;

namespace OnlineShopping.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _service.GetByIdAsync(id.Value);
            if (user == null) return NotFound();

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            await _service.CreateAsync(user);
            return RedirectToAction(nameof(Index));
        }

        // ----------------- REGISTER -----------------

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
                return View(register);

            if (await _service.UsernameExistsAsync(register.UserName))
            {
                ViewData["ErrorMessage"] = "User already exists.";
                return View(register);
            }

            var user = new User
            {
                UserName = register.UserName,
                Password = register.Password,
                UserType = "normal"
            };

            await _service.CreateAsync(user);

            return RedirectToAction(nameof(Login));
        }

        // ----------------- LOGIN -----------------

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = await _service.LoginAsync(login.UserName, login.Password);

            if (user == null)
            {
                ViewData["ErrorMessage"] = "Invalid username or password.";
                return View(login);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim(ClaimTypes.Role, user.UserType)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("ProductDashboard", "Product");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("ProductDashboard", "Product");
        }

        // ----------------- EDIT -----------------

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(user);

            await _service.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }

        // ----------------- DELETE -----------------

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
