using EcommerceStore.Data;
using EcommerceStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Controllers
{
    public class GRNController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GRNController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GRN/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Products = new SelectList(_context.Products, "ProductID", "Name");
            return View();
        }

        // POST: GRN/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GRNHeader header, List<GRNDetails> details)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (details != null && details.Any(d => d.QuantityReceived > 0))
            {
                var validDetails = details.Where(d => d.QuantityReceived > 0).ToList();

                // 1. Save Header
                header.ReceivedDate = DateTime.Now;
                header.AdminUserID = HttpContext.Session.GetInt32("UserID") ?? 1;

                _context.GRNHeaders.Add(header);
                await _context.SaveChangesAsync();

                // 2. Save Details & Increment Inventory Balance
                foreach (var item in validDetails)
                {
                    item.GRNID = header.GRNID;
                    _context.GRNDetails.Add(item);

                    var product = await _context.Products.FindAsync(item.ProductID);
                    if (product != null)
                    {
                        product.StockQuantity += item.QuantityReceived;
                        _context.Products.Update(product);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("AdminDashboard", "Account");
            }

            ViewBag.Products = new SelectList(_context.Products, "ProductID", "Name");
            return View(header);
        }
    }
}