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
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;

        public UserController(IUserRepository userRepository, IAddressRepository addressRepository)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_userRepository.GetAll());
        }
        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            return Ok(_userRepository.GetByName(name));
        }

        [HttpGet("GetByNameWithAddress")]
        public IActionResult GetByNameWithAddress(string name) 
        {
            return Ok(_userRepository.GetByNameWithAddress(name));
        }

        [HttpGet("GetById")]
        public IActionResult GetById(long id)
        {
            return Ok(_userRepository.GetById(id));
        }

        [HttpPost("Create")]
        public IActionResult Create(UserDto dto)
        {
            if (ModelState.IsValid)
            {
                _userRepository.Add(dto.ToDomain());
                _addressRepository.Add(dto.Address.ToDomain());
                return Ok(dto.ToDomain());
            }
            return BadRequest();
        }

        [HttpPut("Update")]
        public IActionResult Put(UserDto dto)
        {
            if (ModelState.IsValid)
            {
                _userRepository.Update(dto.ToDomain());
                _addressRepository.Update(dto.Address.ToDomain());
                return Ok(dto.ToDomain());
            }
            return BadRequest();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(long id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            _userRepository.Delete(user);
            return Ok("User deleted");
            
        }
    }
}
