using LarsProjekt.Dto;

namespace LarsProjekt.StoreApiAdapter;

public class StoreApiUserOptions
{
    public const string Section = "StoreApiAdapter";

    public List<AppUserDto> Users { get; set; }

}
