namespace EasyPayChallenge.Domain.Models.Dto;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Price { get; set; }
    
    public static ProductDto ConvertFromProduct(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Brand = product.Brand,
            Price = new Money(product.Price.Amount, product.Price.Currency).ToString()
        };
    }
}