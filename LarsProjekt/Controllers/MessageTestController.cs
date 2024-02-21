using LarsProjekt.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers;

public class MessageTestController : Controller
{
    private readonly IMessageService _messageService;

    public MessageTestController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        await _messageService.SendOrder();

        return Ok();
    }
}
