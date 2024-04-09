namespace LarsProjekt.Messages.Dtos;

public class OrderDto
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public decimal Total { get; set; }
    public DateTimeOffset OrderDate { get; set; }   
    public long AddressId { get; set; }
    public List<OrderDetailDto> Details { get; set; }

}

