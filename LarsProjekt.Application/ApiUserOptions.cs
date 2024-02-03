using LarsProjekt.Dto;

namespace LarsProjekt.Application;

public class ApiUserOptions
{
    public const string Section = "Authentication";

    public List<AppUserDto> Users {  get; set; }
    
}
