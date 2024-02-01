using LarsProjekt.Dto;

namespace LarsProjekt.Application;

public class ApiUserOptions
{
    public const string Section = "Authentication:Users";

    public List<AppUserDto> AppUserDtos {  get; set; }
    //public string Key { get; set; }
    //public string Name { get; set; }
}
