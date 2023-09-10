using LarsProjekt.Application;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers;

public class ProductController : Controller
{
    private ProductRepository _productRepository;
    public ProductController(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public IActionResult Index()
    {
        var list = new List<ProductModel>();
        foreach (var product in _productRepository.Products)
        {
            list.Add(new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,
            });
        }

        return View(list);
    }

    public IActionResult Details(long id)
    {
        var product = _productRepository.Products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            var model = new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price
            };

            return View(model);
        }
        else
        {
            throw new Exception("Product not found");
        }
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View(new ProductModel());
    }


    [HttpPost]
    public IActionResult Create(ProductModel productModel)
    {
        if (ModelState.IsValid)
        {
            var product = new Product();
            product.Id = productModel.Id;
            product.Name = productModel.Name;
            product.Category = productModel.Category;
            product.Description = productModel.Description;
            product.Price = productModel.Price;
            var maxId = _productRepository.Products.Max(p => p.Id);
            product.Id = maxId + 1;
            _productRepository.Products.Add(product);
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View();
        }
    }


    [HttpGet]
    public IActionResult Edit(long id)
    {
        var product = _productRepository.Products.FirstOrDefault(p => p.Id == id);
        var model = new ProductModel
        {
            Id = id,
            Name = product.Name,
            Category = product.Category,
            Description = product.Description,
            Price = product.Price,
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(ProductModel productModel)
    {
        if (ModelState.IsValid)
        {
            var product = _productRepository.Products.FirstOrDefault(p => p.Id == productModel.Id);
            product.Name = productModel.Name;
            product.Category = productModel.Category;
            product.Description = productModel.Description;
            product.Price = productModel.Price;

            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View();
        }
    }

    //[HttpGet]
    //public IActionResult CreateEdit(long id)
    //{
    //    if(id == 0)
    //    {
    //        return View(new ProductModel());
    //    } else
    //    {
    //        var product = _productRepository.Products.FirstOrDefault(product => product.Id == id);
    //        var model = new ProductModel
    //        {
    //            Id = id,
    //            Name = product.Name,
    //            Category = product.Category,
    //            Description = product.Description,
    //            Price = product.Price,
    //        };
    //        return View(model);
    //    }

    //}

    //[HttpPost]
    //public IActionResult CreateEdit(ProductModel model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var product = new Product();
    //        product.Name = model.Name;
    //        product.Category = model.Category;
    //        product.Description = model.Description;
    //        product.Price = model.Price;
    //        var maxId = _productRepository.Products.Max(product => product.Id);
    //        product.Id = maxId + 1;
    //        _productRepository.Products.Add(product);
    //        return RedirectToAction(nameof(Index));
    //    }
    //    else
    //    {
    //        return View(model);
    //    }
    //}

    [HttpGet]
    public IActionResult Delete(long id)
    {
        var product = _productRepository.Products.FirstOrDefault(product => product.Id == id);
        var model = new ProductModel
        {
            Id = id,
            Name = product.Name,
            Category = product.Category,
            Description = product.Description,
            Price = product.Price,
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult DeletePost(long id)
    {
        var model = _productRepository.Products.FirstOrDefault(p => p.Id == id);
        _productRepository.Products.Remove(model);
        return RedirectToAction(nameof(Index));
    }
}
