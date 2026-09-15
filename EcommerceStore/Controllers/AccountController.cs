using EcommerceStore.Data;
using EcommerceStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Register
        public IActionResult Register() => View();

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email already registered.");
                    return View(model);
                }

                // Replace 'User' model properties here if named differently in your User.cs
                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    Role = "Customer"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Save session on registration
                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("UserRole", user.Role);

                return RedirectToAction("Create", "SalesOrder");
            }
            return View(model);
        }

        // GET: Account/Login
        public IActionResult Login() => View();

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.PasswordHash == model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserID", user.UserID);
                    HttpContext.Session.SetString("UserName", user.FullName);
                    HttpContext.Session.SetString("UserRole", user.Role ?? "Customer");

                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("AdminDashboard", "Account");
                    }
                    return RedirectToAction("Create", "SalesOrder");
                }
                ModelState.AddModelError("", "Invalid email or password.");
            }
            return View(model);
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: Account/AdminDashboard
        public async Task<IActionResult> AdminDashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login");
            }

            ViewBag.TotalSales = await _context.SalesOrders.SumAsync(s => s.TotalAmount);
            ViewBag.TotalOrders = await _context.SalesOrders.CountAsync();
            ViewBag.UsersCount = await _context.Users.CountAsync();

            var recentOrders = await _context.SalesOrders
                .Include(s => s.Customer)
                .OrderByDescending(s => s.OrderDate)
                .Take(10)
                .ToListAsync();

            return View(recentOrders);
        }
    }
}