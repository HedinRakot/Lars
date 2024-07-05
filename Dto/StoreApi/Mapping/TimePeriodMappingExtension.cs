namespace LarsProjekt.Dto.StoreApi.Mapping;

public static class TimePeriodMappingExtension
{
    public static Domain.StoreApi.TimePeriod ToDomain(this TimePeriodDto dto)
    {
        return new Domain.StoreApi.TimePeriod
        {
            Id = dto.Id,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };
    }

    public static TimePeriodDto ToDto(this Domain.StoreApi.TimePeriod time)
    {
        List<ProductDto> products = [];
        if (time.Products != null)
        {
            foreach (var product in time.Products)
            {
                products.Add(product.ToDto());
            }
        }
        return new TimePeriodDto(
            time.Id,
            time.StartDate,
            time.EndDate,
            products
            );
    }
}
