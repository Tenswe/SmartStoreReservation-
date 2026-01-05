namespace SmartStoreReservation.Core.DTOs;

public class ProductDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
    public string? ImageUrl { get; set; }
    public string ShopName { get; set; } = string.Empty;
    public List<string> Categories { get; set; } = new();
}

public class CreateProductDto
{
    public long ShopId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
    public string? ImageUrl { get; set; }
    public List<long> CategoryIds { get; set; } = new();
}

public class UpdateProductDto
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
    public string? ImageUrl { get; set; }
}