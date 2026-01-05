using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartStoreReservation.Core.DTOs;
using SmartStoreReservation.Core.Entities;
using SmartStoreReservation.Core.Interfaces;

namespace SmartStoreReservation.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetProductsAsync(long? categoryId = null);
    Task<ProductDto?> GetProductByIdAsync(long id);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto);
    Task<ProductDto> UpdateProductAsync(long id, UpdateProductDto dto);
    Task DeleteProductAsync(long id);
}

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ProductDto>> GetProductsAsync(long? categoryId = null)
    {
        var products = await _unitOfWork.Repository<Product>().GetAllAsync();
        
        if (categoryId.HasValue)
        {
            products = products.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId));
        }

        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(long id)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
    {
        _logger.LogInformation("Se creează produs nou: {ProductName}", dto.Name);

        var product = _mapper.Map<Product>(dto);
        product.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<Product>().AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        // Adaugă categoriile
        foreach (var categoryId in dto.CategoryIds)
        {
            var productCategory = new ProductCategory
            {
                ProductId = product.Id,
                CategoryId = categoryId
            };
            await _unitOfWork.Repository<ProductCategory>().AddAsync(productCategory);
        }

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Produs creat cu succes cu ID-ul: {ProductId}", product.Id);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> UpdateProductAsync(long id, UpdateProductDto dto)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product == null) throw new KeyNotFoundException("Produsul nu a fost găsit");

        _mapper.Map(dto, product);
        await _unitOfWork.Repository<Product>().UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task DeleteProductAsync(long id)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product == null) throw new KeyNotFoundException("Produsul nu a fost găsit");

        await _unitOfWork.Repository<Product>().DeleteAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }
}