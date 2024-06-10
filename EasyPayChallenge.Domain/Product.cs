namespace EasyPayChallenge.Domain;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public Money Price { get; set; }
}