using LarsProjekt.Domain;

namespace LarsProjekt.Application;

public class ProductRepository
{  
    public ProductRepository()
    {
        Products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Alienware",
                Category = "Notebook",
                Description = "Power for the latest AAA games",
                Price = 2000
            },
            new Product
            {
                Id= 2,
                Name= "Galaxy Buds",
                Category = "Wearable",
                Description = "Let the music play",
                Price = 150
            },
            new Product
            {
                Id= 3,
                Name="Galaxy S23",
                Category= "Smartphone",
                Description= "With the new and better camera",
                Price= 1500
            }
        };
    }
    public List<Product> Products { get; set; }
}
