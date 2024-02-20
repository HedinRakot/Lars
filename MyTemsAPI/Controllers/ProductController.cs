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
    [Route("products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_productRepository.GetAll());
        }

        [HttpGet("GetById")]
        public IActionResult GetById(long id)
        {
            return Ok(_productRepository.GetById(id));
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] ProductDto productDto)
        {
            var domain = productDto.ToDomain();
            _productRepository.Add(domain);
            var dto = domain.ToDto();
            return Ok(dto);
        }

        [HttpPut("Update")]
        public IActionResult Put([FromBody] ProductDto productDto)
        {
            var domain = productDto.ToDomain();
            _productRepository.Update(domain);
            var dto = domain.ToDto();
            return Ok(dto);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(long id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            _productRepository.Delete(product);
            return Ok("Product deleted");

        }
    }
}
