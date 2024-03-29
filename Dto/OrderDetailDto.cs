﻿using LarsProjekt.Domain;

namespace LarsProjekt.Dto;

public record OrderDetailDto(
    long Id ,
    long OrderId,
    long ProductId,
    int Quantity,
    decimal? UnitPrice,
    decimal DiscountedPrice,
    decimal Discount
    );


