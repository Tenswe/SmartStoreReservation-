using Microsoft.AspNetCore.Mvc;
using SmartStoreReservation.Core.DTOs;
using SmartStoreReservation.Services;
using System.ComponentModel.DataAnnotations;

namespace SmartStoreReservation.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Obține toate produsele cu filtru opțional de categorie
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts([FromQuery] long? categoryId)
    {
        _logger.LogInformation("Se obțin produsele cu categoryId: {CategoryId}", categoryId);
        var products = await _productService.GetProductsAsync(categoryId);
        return Ok(products);
    }

    /// <summary>
    /// Obține produs după ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct([Required] long id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new { message = "Produsul nu a fost găsit" });

        return Ok(product);
    }

    /// <summary>
    /// Creează produs nou
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await _productService.CreateProductAsync(dto);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    /// <summary>
    /// Actualizează produs existent
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct([Required] long id, [FromBody] UpdateProductDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await _productService.UpdateProductAsync(id, dto);
        return Ok(product);
    }

    /// <summary>
    /// Șterge produs
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct([Required] long id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}