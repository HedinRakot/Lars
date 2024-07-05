using LarsProjekt.Dto;

namespace LarsProjekt.OrderApiAdapter;

public class OrderApiUserOptions
{
    public const string Section = "OrderApiAdapter";

    public List<AppUserDto> Users { get; set; }

}
