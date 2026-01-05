using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartStoreReservation.Core.DTOs;
using SmartStoreReservation.Core.Entities;
using SmartStoreReservation.Core.Interfaces;

namespace SmartStoreReservation.Services;

public interface IReservationService
{
    Task<string> CreateReservationAsync(CreateReservationDto dto);
    Task<List<ReservationDto>> GetReservationsAsync();
    Task<List<AvailableCabinDto>> GetAvailableCabinsAsync(long productId, DateTime date, TimeSpan time);
}

public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReservationService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ReservationDto>> GetReservationsAsync()
    {
        var reservations = await _unitOfWork.Repository<Reservation>().GetAllAsync();
        return _mapper.Map<List<ReservationDto>>(reservations);
    }

    public async Task<string> CreateReservationAsync(CreateReservationDto dto)
    {
        _logger.LogInformation("Se creează rezervare pentru utilizatorul {UserId}, produsul {ProductId}", dto.UserId, dto.ProductId);

        var time = TimeSpan.Parse(dto.Hour);
        const int duration = 30; // minute
        var endTime = time.Add(TimeSpan.FromMinutes(duration));

        // Verifică conflictele
        var existingReservations = await _unitOfWork.Repository<Reservation>()
            .FindAsync(r => r.CabinId == dto.CabinId && 
                           r.Date.Date == dto.Date.Date && 
                           r.Status != "Anulată");

        var conflict = existingReservations.Any(r => 
            r.Hour < endTime && r.Hour.Add(TimeSpan.FromMinutes(r.Duration)) > time);

        if (conflict)
        {
            throw new InvalidOperationException("Cabina este deja rezervată pentru acest interval orar.");
        }

        // Validează că entitățile există
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(dto.ProductId);
        if (product == null) throw new KeyNotFoundException("Produsul nu a fost găsit");

        var cabin = await _unitOfWork.Repository<Cabin>().GetByIdAsync(dto.CabinId);
        if (cabin == null) throw new KeyNotFoundException("Cabina nu a fost găsită");

        var user = await _unitOfWork.Repository<User>().GetByIdAsync(dto.UserId);
        if (user == null) throw new KeyNotFoundException("Utilizatorul nu a fost găsit");

        var reservation = _mapper.Map<Reservation>(dto);
        reservation.Duration = duration;
        reservation.AccessCode = GenerateAccessCode();
        reservation.Status = "Confirmată";
        reservation.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<Reservation>().AddAsync(reservation);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Rezervare creată cu succes cu codul de acces {AccessCode}", reservation.AccessCode);
        return reservation.AccessCode!;
    }

    public async Task<List<AvailableCabinDto>> GetAvailableCabinsAsync(long productId, DateTime date, TimeSpan time)
    {
        const int duration = 30;
        var endTime = time.Add(TimeSpan.FromMinutes(duration));

        // Obține produsul pentru a găsi magazinul
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
        if (product == null) throw new KeyNotFoundException("Produsul nu a fost găsit");

        // Obține toate cabinele din același magazin
        var cabins = await _unitOfWork.Repository<Cabin>()
            .FindAsync(c => c.ShopId == product.ShopId);

        // Obține rezervările existente pentru data respectivă
        var reservations = await _unitOfWork.Repository<Reservation>()
            .FindAsync(r => r.Date.Date == date.Date && r.Status != "Anulată");

        var availableCabins = new List<AvailableCabinDto>();

        foreach (var cabin in cabins)
        {
            var isAvailable = !reservations.Any(r => 
                r.CabinId == cabin.Id &&
                r.Hour < endTime && 
                r.Hour.Add(TimeSpan.FromMinutes(r.Duration)) > time);

            var cabinDto = _mapper.Map<AvailableCabinDto>(cabin);
            cabinDto.IsAvailable = isAvailable;
            availableCabins.Add(cabinDto);
        }

        return availableCabins;
    }

    private static string GenerateAccessCode()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}