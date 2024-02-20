using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTemsAPI.Authentication;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Dto;
using MyTemsAPI.Dto.Mapping;
using MyTemsAPI.Models.Mapping;

namespace MyTemsAPI.Controllers
{
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationScheme.DefaultScheme)]
    [Route("address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_addressRepository.GetAll());
        }

        [HttpGet("GetById")]
        public IActionResult GetById(long id)
        {
            return Ok(_addressRepository.GetById(id));
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] AddressDto dto)
        {
            if (ModelState.IsValid)
            {
                _addressRepository.Add(dto.ToDomain());
                return Ok(dto.ToDomain());
            }
            return BadRequest();
        }

        [HttpPut("Update")]
        public IActionResult Put([FromBody] AddressDto dto)
        {
            if (ModelState.IsValid)
            {
                _addressRepository.Update(dto.ToDomain());
                return Ok(dto.ToDomain());
            }
            return BadRequest();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(long id)
        {
            var address = _addressRepository.GetById(id);
            if (address == null)
            {
                return NotFound();
            }
            _addressRepository.Delete(address);
            return Ok("Address deleted");
            
        }
    }
}
