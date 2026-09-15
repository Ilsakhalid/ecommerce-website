using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EcommerceStore.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // 0. Force SQL Server connection pool to drop lingering sessions and recreate DB safely
            
            context.Database.EnsureCreated();

            // 1. Admin Seeding / Upgrade
            var adminUser = context.Users.FirstOrDefault(u => u.Email == "admin@store.com");
            if (adminUser != null)
            {
                adminUser.Role = "Admin";
            }
            else
            {
                context.Users.Add(new User
                {
                    FullName = "System Admin",
                    Email = "admin@store.com",
                    PasswordHash = "admin123",
                    Role = "Admin"
                });
            }
            context.SaveChanges();

            if (context.Categories.Any()) return;

            // 2. Categories
            var categories = new Category[]
            {
                new Category { CategoryName = "Electronics", Description = "Gadgets and tech items" },
                new Category { CategoryName = "Apparel", Description = "Clothing and everyday wear" },
                new Category { CategoryName = "Home & Kitchen", Description = "Household appliances" },
                new Category { CategoryName = "Books & Stationeries", Description = "Reference guides and books" }
            };
            context.Categories.AddRange(categories);

            // 3. Business Nature Types
            var natureTypes = new BusinessNatureType[]
            {
                new BusinessNatureType { TypeName = "Retail" },
                new BusinessNatureType { TypeName = "Wholesale" },
                new BusinessNatureType { TypeName = "Direct Import" }
            };
            context.BusinessNatureTypes.AddRange(natureTypes);
            context.SaveChanges();

            // 4. Products
            var products = new Product[]
            {
                // Electronics
                new Product { Name = "Wireless Headphones", PackSize = "1 Unit", UnitPrice = 149.99m, CategoryID = categories[0].CategoryID, NatureTypeID = natureTypes[0].NatureTypeID, ImageURL = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=500&q=80" },
                new Product { Name = "Mechanical Keyboard", PackSize = "1 Unit", UnitPrice = 89.50m, CategoryID = categories[0].CategoryID, NatureTypeID = natureTypes[2].NatureTypeID, ImageURL = "https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=500&q=80" },
                new Product { Name = "Smart Fitness Watch", PackSize = "1 Unit", UnitPrice = 199.00m, CategoryID = categories[0].CategoryID, NatureTypeID = natureTypes[0].NatureTypeID, ImageURL = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=500&q=80" },
                new Product { Name = "Ultra Power Bank 20k", PackSize = "1 Unit", UnitPrice = 39.99m, CategoryID = categories[0].CategoryID, NatureTypeID = natureTypes[1].NatureTypeID, ImageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQPD4HS7jsFuNYhSmEsoIpiVg9vxRxMPsluhjRsgcWfmVgyCfRx7bVNl0U&s=10" },

                // Apparel
                new Product { Name = "Classic Leather Watch", PackSize = "1 Unit", UnitPrice = 120.00m, CategoryID = categories[1].CategoryID, NatureTypeID = natureTypes[0].NatureTypeID, ImageURL = "https://images.unsplash.com/photo-1524805444758-089113d48a6d?w=500&q=80" },
                new Product { Name = "Everyday Canvas Backpack", PackSize = "1 Unit", UnitPrice = 54.00m, CategoryID = categories[1].CategoryID, NatureTypeID = natureTypes[0].NatureTypeID, ImageURL = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80" },
                new Product { Name = "Denim Jacket Classic", PackSize = "1 Unit", UnitPrice = 75.00m, CategoryID = categories[1].CategoryID, NatureTypeID = natureTypes[1].NatureTypeID, ImageURL = "https://images.unsplash.com/photo-1537465978529-d23b17165b3b?q=80&w=1470&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },

                // Home & Kitchen
                new Product { Name = "Espresso Coffee Machine", PackSize = "1 Unit", UnitPrice = 165.00m, CategoryID = categories[2].CategoryID, NatureTypeID = natureTypes[2].NatureTypeID, ImageURL = "https://images.unsplash.com/photo-1517668808822-9ebb02f2a0e6?w=500&q=80" },
                new Product { Name = "Smart Air Purifier", PackSize = "1 Unit", UnitPrice = 95.00m, CategoryID = categories[2].CategoryID, NatureTypeID = natureTypes[0].NatureTypeID, ImageURL = "https://images.unsplash.com/photo-1632928274371-878938e4d825?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                new Product { Name = "Stainless Steel Kettle", PackSize = "1 Unit", UnitPrice = 34.50m, CategoryID = categories[2].CategoryID, NatureTypeID = natureTypes[0].NatureTypeID, ImageURL = "https://www.beka-cookware.com/cdn/shop/files/retro-kettle-beka-cookware_865x1080_590.jpg?v=1719840554&width=1500" },

                // Books & Stationeries
                new Product { Name = "Clean Code Handbook", PackSize = "1 Book", UnitPrice = 45.00m, CategoryID = categories[3].CategoryID, NatureTypeID = natureTypes[0].NatureTypeID, ImageURL = "https://images.unsplash.com/photo-1532012197267-da84d127e765?w=500&q=80" },
                new Product { Name = "Minimalist Leather Journal", PackSize = "1 Unit", UnitPrice = 22.00m, CategoryID = categories[3].CategoryID, NatureTypeID = natureTypes[1].NatureTypeID, ImageURL = "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=500&q=80" },
                new Product { Name = "Harry Potter", PackSize = "1 Book", UnitPrice = 20.00m, CategoryID = categories[3].CategoryID, NatureTypeID = natureTypes[0].NatureTypeID, ImageURL = "https://www.libertybooks.com/image/cache/catalog/9781408855652-626x974.jpg?q6" },
                new Product { Name = "The Hunger Games Book", PackSize = "1 Book", UnitPrice = 20.00m, CategoryID = categories[3].CategoryID, NatureTypeID = natureTypes[0].NatureTypeID, ImageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTAlZU2cPAE4-cFl_J1C3d-AEyfsduqOhNQvNq0Gwe-2j-jwXZ5wnUcdpk&s=10" }

            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}