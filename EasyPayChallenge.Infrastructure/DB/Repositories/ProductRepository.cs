using System.Data;
using Dapper;
using EasyPayChallenge.Application.Repositories;
using EasyPayChallenge.Domain;
using Microsoft.Data.SqlClient;

namespace EasyPayChallenge.Infrastructure.DB.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Product>> GetAll()
        {
            using var db = Connection;
            var sql = "SELECT Id, Name, Brand, PriceAmount, PriceCurrency FROM Products";
            var products = await db.QueryAsync(sql);

            return products.Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Price = new Money(p.PriceAmount, Enum.Parse<Currency>(p.PriceCurrency))
            });
        }

        public async Task<Product> GetById(int id)
        {
            using var db = Connection;
            var sql = "SELECT Id, Name, Brand, PriceAmount, PriceCurrency FROM Products WHERE Id = @Id";
            var product = await db.QueryFirstOrDefaultAsync(sql, new { Id = id });

            if (product == null)
                return null;

            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Price = new Money(product.PriceAmount, Enum.Parse<Currency>(product.PriceCurrency))
            };
        }

        public async Task<int> Create(Product product)
        {
            using var db = Connection;
            var sql = @"INSERT INTO Products (Name, Brand, PriceAmount, PriceCurrency) 
                        VALUES (@Name, @Brand, @PriceAmount, @PriceCurrency); 
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            var parameters = new
            {
                product.Name,
                product.Brand,
                PriceAmount = product.Price.Amount,
                PriceCurrency = product.Price.Currency.ToString()
            };
            return await db.QuerySingleAsync<int>(sql, parameters);
        }

        public async Task<bool> Update(Product product)
        {
            using var db = Connection;
            var sql = @"UPDATE Products SET 
                        Name = @Name, 
                        Brand = @Brand, 
                        PriceAmount = @PriceAmount, 
                        PriceCurrency = @PriceCurrency 
                        WHERE Id = @Id";
            var parameters = new
            {
                product.Name,
                product.Brand,
                PriceAmount = product.Price.Amount,
                PriceCurrency = product.Price.Currency.ToString(),
                product.Id
            };
            return await db.ExecuteAsync(sql, parameters) > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var db = Connection;
            var sql = "DELETE FROM Products WHERE Id = @Id";
            return await db.ExecuteAsync(sql, new { Id = id }) > 0;
        }
    }
}
