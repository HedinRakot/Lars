using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTemsAPI.Authentication;
using MyTemsAPI.Database;
using MyTemsAPI.Domain;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Dto;
using MyTemsAPI.Dto.Mapping;
using MyTemsAPI.Models.Mapping;

namespace MyTemsAPI.Controllers;

[Authorize(AuthenticationSchemes = ApiKeyAuthenticationScheme.DefaultScheme)]
[Route("orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ISqlUnitOfWork _unitOfWork;
    //private readonly IOrderRepository _orderRepository;
    //private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly ICouponCountService _couponCountService;

    public OrderController(/*IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, */ICouponCountService couponCountService, ISqlUnitOfWork unitOfWork)
    {
        //_orderRepository = orderRepository;
        //_orderDetailRepository = orderDetailRepository;
        _couponCountService = couponCountService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        return Ok(_unitOfWork.OrderRepository.GetAll());
    }

    [HttpGet("GetById")]
    public IActionResult GetById(long id)
    {
        return Ok(_unitOfWork.OrderRepository.GetById(id));
    }

    [HttpPost("Create")]
    public IActionResult Create(PlaceOrderDto dto)
    {
        _unitOfWork.OrderRepository.Add(dto.Order.ToDomain());
        //foreach( var detail in dto.Order.Details )
        //{
        //    _unitOfWork.OrderDetailRepository.Add(detail.ToDomain());
        //}
        _unitOfWork.SaveChanges();

        if ( dto.Coupons != null )
        {
            foreach (var coupon in dto.Coupons)
            {
                _couponCountService.UpdateCouponCount(coupon.Code);
            }
        }
        return Ok(dto);
    }

    [HttpPut("Update")]
    public IActionResult Put(OrderDto dto)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.OrderRepository.Update(dto.ToDomain());
            return Ok(dto.ToDomain());
        }
        return BadRequest();
    }

    [HttpDelete("Delete")]
    public IActionResult Delete(long id)
    {
        var order = _unitOfWork.OrderRepository.GetById(id);
        if (order == null)
        {
            return NotFound();
        }
        _unitOfWork.OrderRepository.Delete(order);
        return Ok("Order deleted");
        
    }
}
