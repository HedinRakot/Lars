namespace LarsProjekt.Dto.StoreApi
{
    public record OfferDto(
        long Id,
        long ProductId,
        long TimePeriodId,
        decimal? Price
        );
}
