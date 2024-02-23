using Microsoft.AspNetCore.Mvc;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Dto;
using MyTemsAPI.Dto.Mapping;
using MyTemsAPI.Models.Mapping;

namespace MyTemsAPI.Controllers
{
    [Route("coupons")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_couponRepository.GetAll());
        }

        [HttpGet("GetById")]
        public IActionResult GetById(long id)
        {
            return Ok(_couponRepository.GetById(id).ToDto());
        }
        [HttpGet("GetByName")]
        public IActionResult GetByName(string name) 
        {
            return Ok(_couponRepository.GetByName(name).ToDto());
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] CouponDto dto)
        {
            if (ModelState.IsValid)
            {
                _couponRepository.Add(dto.ToDomain());
                return Ok(dto.ToDomain());
            }
            return BadRequest();
        }

        [HttpPut("Update")]
        public IActionResult Put([FromBody] CouponDto dto)
        {
            if (ModelState.IsValid)
            {
                _couponRepository.Update(dto.ToDomain());
                return Ok(dto.ToDomain());
            }
            return BadRequest();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(long id)
        {
            var coupon = _couponRepository.GetById(id);
            if (coupon == null)
            {
                return NotFound();
            }
            _couponRepository.Delete(coupon);
            return Ok("Coupon deleted");
            
        }
    }
}
