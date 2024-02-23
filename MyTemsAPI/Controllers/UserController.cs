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
        public async Task<IActionResult> GetAll() => Ok(await _userRepository.GetAll());

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name) => Ok(await _userRepository.GetByName(name));

        [HttpGet("GetByNameWithAddress")]
        public async Task<IActionResult> GetByNameWithAddress(string name) => Ok(await _userRepository.GetByNameWithAddress(name));

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id) => Ok(await _userRepository.GetById(id));

        [HttpPost("Create")]
        public async Task<IActionResult> Create(UserDto dto)
        {
                _userRepository.Add(dto.ToDomain());
                _addressRepository.Add(dto.Address.ToDomain());
                return Ok(dto.ToDomain());
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Put(UserDto dto)
        {
                _userRepository.Update(dto.ToDomain());
                _addressRepository.Update(dto.Address.ToDomain());
                return Ok(dto.ToDomain());
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userRepository.Delete(user);
            return Ok();
            
        }
    }
}
