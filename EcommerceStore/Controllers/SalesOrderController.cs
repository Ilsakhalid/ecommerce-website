using EcommerceStore.Data;
using EcommerceStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Controllers
{
    public class SalesOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SalesOrder/Create (Modern Storefront Catalog & Cart)
        public async Task<IActionResult> Create(int? natureTypeId, int? categoryId, string searchTerm, string sortBy)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserID");
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.BusinessNatureType)
                .AsQueryable();

            // Filtering Logic
            if (natureTypeId.HasValue && natureTypeId.Value > 0)
                query = query.Where(p => p.NatureTypeID == natureTypeId.Value);

            if (categoryId.HasValue && categoryId.Value > 0)
                query = query.Where(p => p.CategoryID == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(p => p.Name.Contains(searchTerm) || (p.Category != null && p.Category.CategoryName.Contains(searchTerm)));

            // Sorting Logic
            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.UnitPrice),
                "price_desc" => query.OrderByDescending(p => p.UnitPrice),
                "name_asc" => query.OrderBy(p => p.Name),
                _ => query.OrderBy(p => p.ProductID)
            };

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.NatureTypes = await _context.BusinessNatureTypes.ToListAsync();
            ViewBag.SelectedNature = natureTypeId;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortBy = sortBy;

            var products = await query.ToListAsync();
            return View(products);
        }

        // POST: SalesOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesOrder order, List<SalesOrderDetails> details)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserID");
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fallback: Extract directly from form if model binding missed details
            if (details == null || !details.Any())
            {
                details = new List<SalesOrderDetails>();
                var form = Request.Form;

                for (int i = 0; form.ContainsKey($"details[{i}].ProductID"); i++)
                {
                    if (int.TryParse(form[$"details[{i}].ProductID"], out int productId) &&
                        int.TryParse(form[$"details[{i}].QuantitySold"], out int quantity) &&
                        decimal.TryParse(form[$"details[{i}].UnitPrice"], out decimal unitPrice))
                    {
                        details.Add(new SalesOrderDetails
                        {
                            ProductID = productId,
                            QuantitySold = quantity,
                            UnitPrice = unitPrice
                        });
                    }
                }
            }

            if (details != null && details.Any(d => d.QuantitySold > 0))
            {
                var validDetails = details.Where(d => d.QuantitySold > 0).ToList();

                // 1. Check stock availability
                foreach (var item in validDetails)
                {
                    var product = await _context.Products.FindAsync(item.ProductID);
                    if (product == null || product.StockQuantity < item.QuantitySold)
                    {
                        TempData["ErrorMessage"] = $"Insufficient stock for product: {product?.Name ?? "Selected Item"}";
                        return RedirectToAction(nameof(Create));
                    }
                }

                // 2. Save Header Order
                order.CustomerID = currentUserId.Value;
                order.OrderDate = DateTime.Now;
                order.TotalAmount = validDetails.Sum(d => d.QuantitySold * d.UnitPrice);

                _context.SalesOrders.Add(order);
                await _context.SaveChangesAsync();

                // 3. Save Details & Deduct Inventory Balance
                foreach (var item in validDetails)
                {
                    item.OrderID = order.OrderID;
                    _context.SalesOrderDetails.Add(item);

                    var product = await _context.Products.FindAsync(item.ProductID);
                    if (product != null)
                    {
                        product.StockQuantity -= item.QuantitySold;
                        _context.Products.Update(product);
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Confirmation", new { id = order.OrderID });
            }

            TempData["ErrorMessage"] = "Your cart is empty or contained invalid items.";
            return RedirectToAction(nameof(Create));
        }

        // GET: SalesOrder/Confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _context.SalesOrders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null) return NotFound();

            var orderDetails = await _context.SalesOrderDetails
                .Include(d => d.Product)
                .Where(d => d.OrderID == id)
                .ToListAsync();

            ViewBag.Details = orderDetails;
            return View(order);
        }

        // GET: SalesOrder/MyOrders
        public async Task<IActionResult> MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Load orders with associated detail items and product info explicitly
            var orders = await _context.SalesOrders
                .Where(s => s.CustomerID == userId)
                .OrderByDescending(s => s.OrderDate)
                .ToListAsync();

            var orderIds = orders.Select(o => o.OrderID).ToList();

            var allDetails = await _context.SalesOrderDetails
                .Include(d => d.Product)
                .Where(d => orderIds.Contains(d.OrderID))
                .ToListAsync();

            ViewBag.AllOrderDetails = allDetails;

            return View(orders);
        }
    }
}