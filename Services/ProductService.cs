using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTOs;
using TechGearAPI.Models;

namespace TechGearAPI.Services
{
    public class ProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<Product>> GetAllProducts()
        {
            return await _db.Products.ToListAsync();
        }

        
        public async Task<Product> CreateProduct(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                Category = dto.Category,
            };
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
       }
        public async Task<List<Product>> GetAllProductsbyId(int id)
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<Product?> UpdateProduct(int id, CreateProductDto dto)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return null;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.Category = dto.Category;

            await _db.SaveChangesAsync();

            return product;
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _db.Products.FindAsync(id);

            if (product == null)
                return false;

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<List<Product>> SearchProducts(string term)
        {
            term = term.Trim().ToLower();

            return await _db.Products
                .Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    p.Description.ToLower().Contains(term) ||
                    p.Category.ToLower().Contains(term))
                .ToListAsync();
        }
        public async Task<List<Product>> SortProducts(string by ,string order)
        {
            IQueryable<Product> query = _db.Products;

            if(by == "price")
            {
                query = order == "desc"
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price);
            }
            else if(by == "name")
            {
                query = order == "asc"
                    ?query.OrderByDescending(p => p.Name)
                    :query.OrderBy(p => p.Name);
            }
            return await query.ToListAsync();
        }
        public async Task<List<Product>> GetProductsPaginated(int page, int pageSize)
        {
            return await _db.Products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }

}
