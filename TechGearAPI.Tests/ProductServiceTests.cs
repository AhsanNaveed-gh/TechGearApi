using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTOs;
using TechGearAPI.Models;
using TechGearAPI.Services;
using Xunit;

namespace TechGearAPI.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateProduct_AddsProductToDatabase()
        {
            var db = GetDbContext();
            var service = new ProductService(db);
            var dto = new CreateProductDto
            {
                Name = "Test Mouse",
                Description = "Wireless gaming mouse",
                Price = 29.99,
                StockQuantity = 50,
                Category = "Peripherals"
            };

            var result = await service.CreateProduct(dto);

            Assert.NotNull(result);
            Assert.Equal("Test Mouse", result.Name);
            Assert.Equal(1, await db.Products.CountAsync());
        }

        [Fact]
        public async Task GetAllProducts_ReturnsAllSeededProducts()
        {
            var db = GetDbContext();
            db.Products.AddRange(
                new Product { Name = "Mouse", Description = "d", Price = 10, StockQuantity = 5, Category = "Peripherals" },
                new Product { Name = "Keyboard", Description = "d", Price = 20, StockQuantity = 5, Category = "Peripherals" }
            );
            await db.SaveChangesAsync();

            var service = new ProductService(db);

            var result = await service.GetAllProducts();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateProduct_WithValidId_UpdatesFields()
        {
            var db = GetDbContext();
            var product = new Product { Name = "Old Name", Description = "old", Price = 10, StockQuantity = 5, Category = "Cat" };
            db.Products.Add(product);
            await db.SaveChangesAsync();

            var service = new ProductService(db);
            var dto = new CreateProductDto { Name = "New Name", Description = "new", Price = 15, StockQuantity = 8, Category = "Cat" };

            var result = await service.UpdateProduct(product.Id, dto);

            Assert.NotNull(result);
            Assert.Equal("New Name", result!.Name);
            Assert.Equal(15, result.Price);
        }

        [Fact]
        public async Task UpdateProduct_WithInvalidId_ReturnsNull()
        {
            var db = GetDbContext();
            var service = new ProductService(db);
            var dto = new CreateProductDto { Name = "X", Description = "X", Price = 1, StockQuantity = 1, Category = "X" };

            var result = await service.UpdateProduct(999, dto);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteProduct_WithValidId_RemovesProductAndReturnsTrue()
        {
            var db = GetDbContext();
            var product = new Product { Name = "ToDelete", Description = "d", Price = 5, StockQuantity = 5, Category = "Cat" };
            db.Products.Add(product);
            await db.SaveChangesAsync();

            var service = new ProductService(db);

            var result = await service.DeleteProduct(product.Id);

            Assert.True(result);
            Assert.Equal(0, await db.Products.CountAsync());
        }

        [Fact]
        public async Task DeleteProduct_WithInvalidId_ReturnsFalse()
        {
            var db = GetDbContext();
            var service = new ProductService(db);

            var result = await service.DeleteProduct(999);

            Assert.False(result);
        }

        [Fact]
        public async Task SearchProducts_FindsMatchByName()
        {
            var db = GetDbContext();
            db.Products.AddRange(
                new Product { Name = "Gaming Mouse", Description = "d", Price = 10, StockQuantity = 5, Category = "Peripherals" },
                new Product { Name = "Office Chair", Description = "d", Price = 10, StockQuantity = 5, Category = "Furniture" }
            );
            await db.SaveChangesAsync();

            var service = new ProductService(db);

            var result = await service.SearchProducts("mouse");

            Assert.Single(result);
            Assert.Equal("Gaming Mouse", result[0].Name);
        }
    }
}