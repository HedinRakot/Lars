using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTemsAPI.Authentication;
using MyTemsAPI.Domain.IRepositories;

namespace MyTemsAPI.Controllers
{
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationScheme.DefaultScheme)]
    [Route("orderdetails")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepository _orderdetailRepository;

        public OrderDetailController(IOrderDetailRepository orderdetailRepository)
        {
            _orderdetailRepository = orderdetailRepository;
        }

        [HttpGet("GetWithOrderId")]
        public IActionResult GetWithOrderId(long id)
        {
            return Ok(_orderdetailRepository.GetListWithOrderId(id));
        }
    }
}
