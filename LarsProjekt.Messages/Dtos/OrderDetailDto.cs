namespace LarsProjekt.Messages.Dtos;

public class OrderDetailDto
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int ProductAmount { get; set; }
    public decimal UnitPrice { get; set; }
}


