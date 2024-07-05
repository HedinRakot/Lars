using LarsProjekt.Dto;

namespace LarsProjekt.UserApiAdapter;

public class ApiUserOptions
{
    public const string Section = "UserApiAdapter";

    public List<AppUserDto> Users { get; set; }

}
