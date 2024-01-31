namespace LarsProjekt.Dto;

public record OrderDto(
    long Id ,
    decimal? Total ,
    DateTimeOffset Date 
    );


