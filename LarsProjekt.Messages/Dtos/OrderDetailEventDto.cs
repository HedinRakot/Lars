﻿namespace LarsProjekt.Messages.Dtos;

public class OrderDetailEventDto
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public decimal Discount { get; set;}
}


