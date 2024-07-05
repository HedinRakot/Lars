namespace LarsProjekt.Domain.StoreApi;

public class Category
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; } = [];
}
