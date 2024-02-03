using LarsProjekt.Application;
using LarsProjekt.Dto.Mapping;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers;
public class ProductController : Controller
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    public async Task<IActionResult> Index()
    {
        var models = new List<ProductModel>();
        var list = await _productService.GetProducts();
        foreach (var product in list)
        {
            models.Add(product.ToModel());
        }

        return View(models);
    }

    public async Task<IActionResult> Details(long id)
    {
        var product = await _productService.GetById(id);
        return View(product.ToModel());
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(long id)
    {
        await _productService.Delete(id);
        return Ok(new { success = "true" });

    }

    [HttpGet]
    public async Task<IActionResult> CreateEdit(long id)
    {
        if (id == 0)
        {
            return View(new ProductModel());
        }
        else
        {
            var product = await _productService.GetById(id);
            return View(product.ToModel());
        }
    }

    [HttpGet]
    public async Task<IActionResult> AddPic(long id)
    {
        var product = await _productService.GetById(id);
        return View(product.ToModel());
    }

    [HttpPut]
    public async Task<IActionResult> AddPic([FromForm] IFormFile file, [FromRoute] long id)
    {
        if (file != null)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var str = Convert.ToBase64String(ms.ToArray());
            var product = await _productService.GetById(id);
            product.Image = str;
            await _productService.Update(product);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult CreateEdit(ProductModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Id == 0)
            {
                _productService.Create(model.ToDomain());
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _productService.Update(model.ToDomain());
                return RedirectToAction(nameof(Index));
            }
        }
        return BadRequest();
    }
}


