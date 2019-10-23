using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using ProductsManagement.Repositories;

namespace ProductsManagement.Controllers
{
    public class ProductApiController : Controller
  {
    private readonly IProductRepository productRepository;
    public ProductApiController(IProductRepository productRepository/*, IStringLocalizer<ProductController> localizer*/)
    {
      this.productRepository = productRepository;
    }


    public IActionResult Index()
    {
        var products = productRepository.getAllProducts();
        return Json(products);
    }


    [HttpGet]
    public object productsJson(DataSourceLoadOptions loadOptions)
    {
      return DataSourceLoader.Load(productRepository.getAllProducts(), loadOptions);
    }
  }
}