using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTOs;
using TechGearAPI.Models;
using TechGearAPI.Services;
using Microsoft.AspNetCore.Authorization;


namespace TechGearAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ProductService _productService;
    public ProductController(AppDbContext db, ProductService productService)
    {
        _db = db;
        _productService = productService;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }

    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {
        var product = await _productService.CreateProduct(dto);
        return CreatedAtAction(nameof(GetProductbyId), new { id = product.Id }, product);


    }
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, CreateProductDto dto)
    {
        var product = await _productService.UpdateProduct(id, dto);
        if (product == null)
            return NotFound();
        return Ok(product);
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductbyId(int id)
    {
        var product = await _productService.GetAllProductsbyId(id);

        if (product == null) 
        {
            return NotFound();
        }
        return Ok(product);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productService.DeleteProduct(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts([FromQuery] string term)
    {
        var products = await _productService.SearchProducts(term);
        return Ok(products);
    }
    
    [HttpGet("sort")]
    public async Task<IActionResult> sortProducts ([FromQuery] string by, [FromQuery] string order)
    {
        var products = await _productService.SortProducts(by, order);
        return Ok(products);
    }
    
    [HttpGet("paged")]
    public async Task<IActionResult> GetProductsPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
    {
        var products = await _productService.GetProductsPaginated(page, pageSize);
        return Ok(products);
    }
}

