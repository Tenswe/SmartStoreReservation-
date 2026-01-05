namespace SmartStoreReservation.Core.Entities;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Shop : BaseEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
}

public class Category : BaseEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ProductCategory> ProductCategories { get; set; } = new();
}

public class Product : BaseEntity
{
    public long Id { get; set; }
    public long ShopId { get; set; }
    public Shop? Shop { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Size { get; set; }
    public string? Color { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public List<ProductCategory> ProductCategories { get; set; } = new();
}

public class ProductCategory
{
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    public long CategoryId { get; set; }
    public Category? Category { get; set; }
}

public class User : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Measurements { get; set; }
    public string? StylePreferences { get; set; }
}

public class Cabin : BaseEntity
{
    public long Id { get; set; }
    public long ShopId { get; set; }
    public Shop? Shop { get; set; }
    public int CabinNumber { get; set; }
}

public class Reservation : BaseEntity
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    public long CabinId { get; set; }
    public Cabin? Cabin { get; set; }
    public DateTime Date { get; set; } // Only Date part
    public TimeSpan Hour { get; set; } // Time of day
    public string? AccessCode { get; set; }
    public int Duration { get; set; } = 30; // Minutes
    public string Status { get; set; } = "Pending";
}
