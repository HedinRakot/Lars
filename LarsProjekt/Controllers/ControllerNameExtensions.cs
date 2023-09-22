using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers;

public static class ControllerNameExtensions
{
    public static string GetControllerName(this Type controller)
    {
        return controller.Name.Replace("Controller", "");
    }
}
