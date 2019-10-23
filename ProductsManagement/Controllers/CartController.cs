using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductsManagement.Models;
using ProductsManagement.Repositories;

namespace ProductsManagement.Controllers
{
  public class CartController : Controller
  {
    public const string SESSION_CART = "SESSION_CART";

    private readonly IProductRepository productRepository;
    private readonly OrderRepository orderRepository;

    public CartController(IProductRepository productRepository, OrderRepository orderRepository)
    {
      this.productRepository = productRepository;
      this.orderRepository = orderRepository;
    }


    public IActionResult Index()
    {
      var deserializedJson = JsonConversion();
      //var productIds = deserializedJson.Select(i => i.productId).ToList();

      //var products = productRepository
      //  .getAllProducts()
      //  .Where(p => productIds.Contains(p.id))
      //  .ToList();

      //var cart = new CartVM
      //{
      //  productId = deserializedJson.Select(i => i.productId).First(),
       
      //};
        
      return View(deserializedJson);
    }


    public IActionResult AddToCart(int id)
    {
      //var cartJson = HttpContext.Session.GetString(SESSION_CART);

      //List<CartVM> deserializedJson = new List<CartVM>();

      //if (!String.IsNullOrEmpty(cartJson))
      //{
      //  deserializedJson = JsonConvert.DeserializeObject<List<CartVM>>(cartJson);
      //}

      List<CartVM> deserializedJson = new List<CartVM>();
       deserializedJson = JsonConversion();

      var searchProdId = deserializedJson.FirstOrDefault(x=>x.productId == id);

      if (searchProdId == null)
      {
        var cart = new CartVM
        {
          productId = id,
          quantity = 1,
          price = productRepository.getProductById(id).price,
          description = productRepository.getProductById(id).description
          //description = productRepository.getProductById(id).description
        };

        deserializedJson.Add(cart);
      }
      else
      {
        searchProdId.quantity++;        
      };
      
      var serializedJson = JsonConvert.SerializeObject(deserializedJson);

      HttpContext.Session.SetString(SESSION_CART, serializedJson);

      return RedirectToAction("Index", "Product", new { message = "Product added successfully!" });
    }


    public IActionResult RemoveFromCart(int id)
    {
      //var cartJson = HttpContext.Session.GetString(SESSION_CART);

      //List<int> deserializedJson = new List<int>();

      //if (!String.IsNullOrEmpty(cartJson))
      //{
      //  deserializedJson = JsonConvert.DeserializeObject<List<int>>(cartJson);
      //}

      //deserializedJson.RemoveAll(i => i == id);

      var deserializedJson = JsonConversion();
      deserializedJson.RemoveAll(i => i.productId == id);

      var serializedJson = JsonConvert.SerializeObject(deserializedJson);

      HttpContext.Session.SetString(SESSION_CART, serializedJson);

      return RedirectToAction("Index");
    }


    //public IActionResult BuyCart(CartVM[] cartVMs)
    //{
    //  foreach (var item in cartVMs)
    //  {
    //    var order = new Order
    //    {
    //      product_id = item.productId,
    //      user_id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
    //      creationDate = DateTime.Now,
    //      quantity = item.quantity
    //    };

    //    orderRepository.placeOrder(order);
    //  }

    //  return RedirectToAction("Index");
    //}


    public IActionResult BuyAll(List<CartVM> cart)
    {
      foreach (var item in cart)
      {
        var order = new Order
        {
          product_id = item.productId,
          user_id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
          creationDate = DateTime.Now,
          quantity = item.quantity
        };

        orderRepository.placeOrder(order);
      }

      return RedirectToAction("Index");

    }


    private List<CartVM> JsonConversion()
    {
      var cartJson = HttpContext.Session.GetString(SESSION_CART);

      if (!String.IsNullOrEmpty(cartJson))
      {
        List<CartVM> deserializedJson = JsonConvert.DeserializeObject<List<CartVM>>(cartJson);
        return deserializedJson;
      }
      return new List<CartVM>();
    }

  }
}