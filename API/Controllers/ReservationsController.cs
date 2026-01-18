using Microsoft.AspNetCore.Mvc;
using SmartStoreReservation.Core.DTOs;
using SmartStoreReservation.Services;
using System.ComponentModel.DataAnnotations;

namespace SmartStoreReservation.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(IReservationService reservationService, ILogger<ReservationsController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }

    /// <summary>
    /// Creează rezervare nouă
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> CreateReservation([FromBody] CreateReservationDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("Se creează rezervare pentru utilizatorul {UserId}", request.UserId);
        
        var accessCode = await _reservationService.CreateReservationAsync(request);
        return Ok(new { AccessCode = accessCode, Message = "Rezervare confirmată." });
    }

    /// <summary>
    /// Obține toate rezervările
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ReservationDto>>> GetReservations()
    {
        var reservations = await _reservationService.GetReservationsAsync();
        return Ok(reservations);
    }

    /// <summary>
    /// Obține rezervările unui utilizator specific
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<ReservationDto>>> GetUserReservations(Guid userId)
    {
        _logger.LogInformation("Se obțin rezervările pentru utilizatorul {UserId}", userId);
        var reservations = await _reservationService.GetUserReservationsAsync(userId);
        return Ok(reservations);
    }

    /// <summary>
    /// Anulează o rezervare
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> CancelReservation(long id)
    {
        _logger.LogInformation("Se anulează rezervarea {ReservationId}", id);
        await _reservationService.CancelReservationAsync(id);
        return Ok(new { Message = "Rezervarea a fost anulată cu succes." });
    }

    /// <summary>
    /// Obține cabinele disponibile pentru un produs, dată și oră specifice
    /// </summary>
    [HttpGet("available-cabins")]
    public async Task<ActionResult<List<AvailableCabinDto>>> GetAvailableCabins(
        [FromQuery, Required] long productId,
        [FromQuery, Required] DateTime date,
        [FromQuery, Required] string hour)
    {
        if (!TimeSpan.TryParse(hour, out var time))
            return BadRequest(new { message = "Format oră invalid. Folosește formatul HH:mm." });

        var cabins = await _reservationService.GetAvailableCabinsAsync(productId, date, time);
        return Ok(cabins);
    }
}