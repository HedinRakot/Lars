namespace LarsProjekt.Domain;

public class Cart
{    
    public int RecordId { get; set; }
    public string CartId { get; set; }
    public int ProductId { get; set; }
    public int Count { get; set; }
    public DateTime DateCreated { get; set; }
    public Product Product { get; set; }
}
