using LarsProjekt.Authentication;
using LarsProjekt.Database.Repositories;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers;
public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationScheme.DefaultScheme)]
    public IActionResult Index()
    {
        var list = new List<ProductModel>();
        foreach (var product in _productRepository.GetAll())
        {
            list.Add(product.ToModel());
        }

        return View(list);
    }

    public IActionResult Details(long id)
    {
        var product = _productRepository.Get(id);
       
        try
        {
            var model = product.ToModel();
            return View(model);
        }
        catch
        {
            throw new Exception("Product not found");
        }
    }

    [HttpGet]
    public IActionResult CreateEdit(long id)
    {
        if (id == 0)
        {
            return View(new ProductModel());
        }
        else
        {
            var product = _productRepository.Get(id);
            var model = product.ToModel();
            return View(model);           
        }
    }

    [HttpGet]
    public IActionResult AddPic(long id)
    {
        return View(_productRepository.Get(id).ToModel());
    }

    [HttpPost]
    public IActionResult AddPic([FromForm] IFormFile file, [FromRoute] long id)
    {
        if (file != null)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var str = Convert.ToBase64String(ms.ToArray());
            var product = _productRepository.Get(id);
            product.Image = str;
            _productRepository.Update(product);
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
                var product = model.ToDomain();
                _productRepository.Add(product);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var product = model.ToDomain();
                _productRepository.Update(product);
                return RedirectToAction(nameof(Details), new { Id = product.Id });
            }
        }       
        return View(model);        
    }

    [HttpDelete]
    public IActionResult Delete(long id)
    {
        try
        {
            var model = _productRepository.Get(id);
            _productRepository.Delete(model);
            return Ok(new { success = "true" });
        } 
        catch 
        { 
            throw new BadHttpRequestException("Oops, please try again!", 400);
        }
    }
}