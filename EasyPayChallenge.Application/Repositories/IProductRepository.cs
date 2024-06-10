using EasyPayChallenge.Domain;

namespace EasyPayChallenge.Application.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<Product> GetById(int id);
    Task<int> Create(Product product);
    Task<bool> Update(Product product);
    Task<bool> Delete(int id);
}