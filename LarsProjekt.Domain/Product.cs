using System.Security.Cryptography.X509Certificates;

namespace LarsProjekt.Domain;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; } 
    public int Price { get; set; }

}
