using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ProductsManagement.Hubs;
using ProductsManagement.Models;
using ProductsManagement.Repositories;

namespace ProductsManagement.Controllers
{
  public class ProductController : Controller
  {
    public const string SESSION_COUNTER = "SESSION_COUNTER";

    private readonly IProductRepository productRepository;
    private readonly IHubContext<ChatHub> hubContext;
    private readonly ProductOptions productOptions;
      //private readonly IStringLocalizer<ProductController> localizer;

    public ProductController(IProductRepository productRepository, IHubContext<ChatHub> hubContext, IOptionsMonitor<ProductOptions> productOptions)
    {
      this.productRepository = productRepository;
      this.hubContext = hubContext;
      this.productOptions = productOptions.CurrentValue;
      //localizer = localizer;
    }

    public IActionResult Index(string filter = null, string message = null)
    {

      var counter = HttpContext.Session.GetInt32(SESSION_COUNTER);

      if (!counter.HasValue)
      {
        counter = 0;
      }
      counter++;
      HttpContext.Session.SetInt32(SESSION_COUNTER, counter.Value);

      ViewData["Counter"] = counter;
      ViewData["message"] = message;
      var view = productOptions.showCards ? "CardIndex" : "Index";
      var search = string.IsNullOrEmpty(filter) ? productRepository.getAllProducts(productOptions.maxProductsCount) : productRepository.getAllProductsByDescription(filter);
      return View(view, search);
    }


    public IActionResult Details(int id)
    {
      return View(productRepository.getProductById(id));
    }

    [Authorize(Policy = "admin")]
    public IActionResult Edit(int id)
    {
      return View(productRepository.getProductById(id));
    }

    [Authorize(Policy = "admin")]
    [HttpPost]
    public IActionResult PostEdit(Product product)
    {
      //var product = new Product(id, description, price);
      productRepository.editProduct(product);
      return RedirectToAction("Index");
    }

    [Authorize(Policy = "admin")]
    public IActionResult Delete(int id)
    {
      return View(productRepository.getProductById(id));
    }

    [Authorize(Policy = "admin")]
    [HttpPost]
    public IActionResult PostDelete(int id)
    {
      productRepository.deleteProductById(id);
      return RedirectToAction("Index");
    }

    [Authorize(Policy = "admin")]
    public IActionResult Create()
    {
      return View();
    }

    [Authorize(Policy = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PostCreate(Product product)
    {

      if (ModelState.IsValid)
      {
        //var  prod1 = new Product(id,description,price); 
        productRepository.createProduct(product);
        hubContext.Clients.All.SendAsync("OnProductCreated");
        return RedirectToAction("Index");
      }
      return View("Create");
    }

    //[HttpPost]
    //public IActionResult SearchResult(string description)
    //{
    //    productRepository.getAllProductsByDescription(description);
    //    return View();


    //[HttpPost]
    //public IActionResult PostCreate(int id, string description, float price)
    //{
    //    var prod1 = new Product(id, description, price);
    //    productRepository.createProduct(prod1);
    //    return RedirectToAction("Index");
    //}

  }
}