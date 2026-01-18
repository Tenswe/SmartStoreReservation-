using Microsoft.EntityFrameworkCore;
using SmartStoreReservation.Core.Entities;
using System.Security.Cryptography;
using System.Text;

namespace SmartStoreReservation.Data;

public class SeedDataService
{
    private readonly AppDbContext _context;

    public SeedDataService(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Verifică dacă datele există deja
        if (await _context.Shops.AnyAsync())
        {
            return; // Datele există deja
        }

        // 1. Adaugă Shops
        var shops = new List<Shop>
        {
            new() { Name = "SmartStore Premium București", Location = "Calea Victoriei 120, București" },
            new() { Name = "SmartStore Luxury Cluj", Location = "Strada Memorandumului 28, Cluj-Napoca" },
            new() { Name = "SmartStore Elite Timișoara", Location = "Piața Unirii 1, Timișoara" }
        };
        _context.Shops.AddRange(shops);
        await _context.SaveChangesAsync();

        // 2. Adaugă Categories
        var categories = new List<Category>
        {
            new() { Name = "Rochii Elegante" },
            new() { Name = "Costume Business" },
            new() { Name = "Ținute Casual" },
            new() { Name = "Accesorii Premium" },
            new() { Name = "Încălțăminte Designer" },
            new() { Name = "Genți de Lux" }
        };
        _context.Categories.AddRange(categories);
        await _context.SaveChangesAsync();

        // 3. Adaugă Users
        var users = new List<User>
        {
            new() 
            { 
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Ana Popescu", 
                Email = "ana.popescu@email.com",
                PasswordHash = HashPassword("parola123"),
                Measurements = "M: 38, Înălțime: 165cm",
                StylePreferences = "Elegant, Modern"
            },
            new() 
            { 
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Maria Ionescu", 
                Email = "maria.ionescu@email.com",
                PasswordHash = HashPassword("parola123"),
                Measurements = "M: 40, Înălțime: 170cm",
                StylePreferences = "Business, Clasic"
            },
            new() 
            { 
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Elena Dumitrescu", 
                Email = "elena.dumitrescu@email.com",
                PasswordHash = HashPassword("parola123"),
                Measurements = "M: 36, Înălțime: 160cm",
                StylePreferences = "Casual, Trendy"
            }
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // 4. Adaugă Cabins
        var cabins = new List<Cabin>();
        
        // București (4 cabine)
        for (int i = 1; i <= 4; i++)
        {
            cabins.Add(new Cabin { ShopId = shops[0].Id, CabinNumber = i });
        }
        
        // Cluj (3 cabine)
        for (int i = 1; i <= 3; i++)
        {
            cabins.Add(new Cabin { ShopId = shops[1].Id, CabinNumber = i });
        }
        
        // Timișoara (5 cabine)
        for (int i = 1; i <= 5; i++)
        {
            cabins.Add(new Cabin { ShopId = shops[2].Id, CabinNumber = i });
        }
        
        _context.Cabins.AddRange(cabins);
        await _context.SaveChangesAsync();

        // 5. Adaugă Products
        var products = new List<Product>
        {
            // București - Rochii Elegante
            new() 
            { 
                ShopId = shops[0].Id, 
                Name = "Rochie Florală de Vară", 
                Size = "M", 
                Color = "Roz Pudră", 
                Stock = 5, 
                Price = 299.99m, 
                ImageUrl = "https://images.unsplash.com/photo-eine-frau-in-einem-kleid-mit-blauem-blumenprint-FGOpeBaiklg?w=800&h=1000&fit=crop" 
            },
            new() 
            { 
                ShopId = shops[0].Id, 
                Name = "Rochie de Seară Neagră", 
                Size = "S", 
                Color = "Negru", 
                Stock = 3, 
                Price = 599.99m, 
                ImageUrl = "https://images.unsplash.com/photo-a-woman-posing-for-a-picture-in-a-black-dress-zj3GjVWzCIE?w=800&h=1000&fit=crop" 
            },
            new() 
            { 
                ShopId = shops[0].Id, 
                Name = "Rochie Cocktail Albastră", 
                Size = "L", 
                Color = "Albastru Royal", 
                Stock = 4, 
                Price = 449.99m, 
                ImageUrl = "https://images.unsplash.com/photo-eine-frau-in-einem-kleid-mit-blauem-blumenprint-FGOpeBaiklg?w=800&h=1000&fit=crop" 
            },
            
            // București - Costume Business
            new() 
            { 
                ShopId = shops[0].Id, 
                Name = "Costum Business Feminin", 
                Size = "M", 
                Color = "Gri Antracit", 
                Stock = 6, 
                Price = 799.99m, 
                ImageUrl = "https://images.unsplash.com/photo-happy-cheerful-business-lady-in-blue-formal-clothes-blJ3pjjgwZ0?w=800&h=1000&fit=crop" 
            },
            new() 
            { 
                ShopId = shops[0].Id, 
                Name = "Blazer Premium cu Pantaloni", 
                Size = "S", 
                Color = "Bleumarin", 
                Stock = 4, 
                Price = 699.99m, 
                ImageUrl = "https://images.unsplash.com/photo-a-woman-in-a-blue-suit-is-posing-for-a-picture-Nb3ptC1E37Q?w=800&h=1000&fit=crop" 
            },
            
            // Cluj - Ținute Casual
            new() 
            { 
                ShopId = shops[1].Id, 
                Name = "Jachetă Denim Vintage", 
                Size = "M", 
                Color = "Albastru Deschis", 
                Stock = 8, 
                Price = 189.99m, 
                ImageUrl = "https://images.unsplash.com/photo-woman-in-blue-denim-jacket-0SOwslg0-YY?w=800&h=1000&fit=crop" 
            },
            new() 
            { 
                ShopId = shops[1].Id, 
                Name = "Pulover Cashmere", 
                Size = "L", 
                Color = "Bej", 
                Stock = 5, 
                Price = 349.99m, 
                ImageUrl = "https://images.unsplash.com/photo-woman-in-brown-knit-sweater-and-white-pants-hPnUtt4LWJo?w=800&h=1000&fit=crop" 
            },
            new() 
            { 
                ShopId = shops[1].Id, 
                Name = "Pantaloni Palazzo", 
                Size = "M", 
                Color = "Verde Oliv", 
                Stock = 6, 
                Price = 229.99m, 
                ImageUrl = "https://images.unsplash.com/photo-a-woman-in-a-woman-in-black-top-and-black-pants-u4vvfCC5mQ0?w=800&h=1000&fit=crop" 
            },
            
            // Timișoara - Mix Premium
            new() 
            { 
                ShopId = shops[2].Id, 
                Name = "Rochie Midi Elegantă", 
                Size = "S", 
                Color = "Bordo", 
                Stock = 3, 
                Price = 399.99m, 
                ImageUrl = "https://images.unsplash.com/photo-eine-frau-in-einem-kleid-mit-blauem-blumenprint-FGOpeBaiklg?w=800&h=1000&fit=crop" 
            },
            new() 
            { 
                ShopId = shops[2].Id, 
                Name = "Costum Trei Piese", 
                Size = "L", 
                Color = "Negru", 
                Stock = 2, 
                Price = 999.99m, 
                ImageUrl = "https://images.unsplash.com/photo-happy-cheerful-business-lady-in-blue-formal-clothes-blJ3pjjgwZ0?w=800&h=1000&fit=crop" 
            },
            new() 
            { 
                ShopId = shops[2].Id, 
                Name = "Rochie Boho Chic", 
                Size = "M", 
                Color = "Crem", 
                Stock = 4, 
                Price = 279.99m, 
                ImageUrl = "https://images.unsplash.com/photo-woman-sits-outdoors-in-a-floral-dress-2j4AtTew4i8?w=800&h=1000&fit=crop" 
            },
            new() 
            { 
                ShopId = shops[2].Id, 
                Name = "Jachetă Piele Premium", 
                Size = "M", 
                Color = "Maro Cognac", 
                Stock = 3, 
                Price = 899.99m, 
                ImageUrl = "https://images.unsplash.com/photo-woman-in-black-jacket-beside-woman-in-blue-denim-jacket-WdcoO8j8okk?w=800&h=1000&fit=crop" 
            }
        };
        
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        // 6. Adaugă Product-Category Relations
        var productCategories = new List<ProductCategory>
        {
            // Rochii Elegante
            new() { ProductId = products[0].Id, CategoryId = categories[0].Id },
            new() { ProductId = products[1].Id, CategoryId = categories[0].Id },
            new() { ProductId = products[2].Id, CategoryId = categories[0].Id },
            new() { ProductId = products[8].Id, CategoryId = categories[0].Id },
            new() { ProductId = products[10].Id, CategoryId = categories[0].Id },
            
            // Costume Business
            new() { ProductId = products[3].Id, CategoryId = categories[1].Id },
            new() { ProductId = products[4].Id, CategoryId = categories[1].Id },
            new() { ProductId = products[9].Id, CategoryId = categories[1].Id },
            
            // Casual
            new() { ProductId = products[5].Id, CategoryId = categories[2].Id },
            new() { ProductId = products[6].Id, CategoryId = categories[2].Id },
            new() { ProductId = products[7].Id, CategoryId = categories[2].Id },
            new() { ProductId = products[11].Id, CategoryId = categories[2].Id },
            
            // Accesorii Premium (jachetă piele)
            new() { ProductId = products[11].Id, CategoryId = categories[3].Id }
        };
        
        _context.ProductCategories.AddRange(productCategories);
        await _context.SaveChangesAsync();

        // 7. Adaugă Sample Reservations
        var reservations = new List<Reservation>
        {
            new() 
            { 
                UserId = users[0].Id, 
                ProductId = products[0].Id, 
                CabinId = cabins[0].Id, 
                Date = DateTime.Today.AddDays(1), 
                Hour = new TimeSpan(14, 30, 0), 
                AccessCode = "ABC123", 
                Duration = 30, 
                Status = "Confirmată" 
            },
            new() 
            { 
                UserId = users[1].Id, 
                ProductId = products[3].Id, 
                CabinId = cabins[1].Id, 
                Date = DateTime.Today.AddDays(1), 
                Hour = new TimeSpan(15, 0, 0), 
                AccessCode = "DEF456", 
                Duration = 30, 
                Status = "Confirmată" 
            },
            new() 
            { 
                UserId = users[2].Id, 
                ProductId = products[5].Id, 
                CabinId = cabins[7].Id, 
                Date = DateTime.Today.AddDays(2), 
                Hour = new TimeSpan(10, 0, 0), 
                AccessCode = "GHI789", 
                Duration = 30, 
                Status = "În Așteptare" 
            }
        };
        
        _context.Reservations.AddRange(reservations);
        await _context.SaveChangesAsync();
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}