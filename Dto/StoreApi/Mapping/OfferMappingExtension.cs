namespace LarsProjekt.Dto.StoreApi.Mapping;

public static class OfferMappingExtension
{
    public static Domain.StoreApi.Offer ToDomain(this OfferDto dto)
    {
        return new Domain.StoreApi.Offer
        {
            Id = dto.Id,
            ProductId = dto.ProductId,
            TimePeriodId = dto.TimePeriodId,
            Price = dto.Price
        };
    }

    public static OfferDto ToDto(this Domain.StoreApi.Offer offer)
    {
        return new OfferDto(
            offer.Id,
            offer.ProductId,
            offer.TimePeriodId,
            offer.Price
            );
    }
}
