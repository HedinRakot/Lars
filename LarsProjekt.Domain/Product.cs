﻿namespace LarsProjekt.Domain;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; } 
    public decimal Price { get; set; }
    public decimal PriceOffer { get; set; }
    public string? Image { get; set; }
}
