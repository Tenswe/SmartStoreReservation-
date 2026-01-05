namespace SmartStoreReservation.Core.DTOs;

public class ReservationDto
{
    public long Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int CabinNumber { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Hour { get; set; }
    public string? AccessCode { get; set; }
    public int Duration { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateReservationDto
{
    public Guid UserId { get; set; }
    public long ProductId { get; set; }
    public long CabinId { get; set; }
    public DateTime Date { get; set; }
    public string Hour { get; set; } = string.Empty; // "14:30" format
}

public class AvailableCabinDto
{
    public long Id { get; set; }
    public int CabinNumber { get; set; }
    public string ShopName { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}