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
      var deserializedJson = deserializeSessionCart();

      foreach (var orderDetails in deserializedJson)
      {
        orderDetails.product = productRepository.getProductById(orderDetails.productId);

      }
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

      List<OrderDetails> deserializedJson = new List<OrderDetails>();
      deserializedJson = deserializeSessionCart();

      var orderDetails = deserializedJson.FirstOrDefault(x => x.productId == id);

      if (orderDetails == null)
      {
        orderDetails = new OrderDetails
        {
          productId = id,
          quantity = 1
        };

        deserializedJson.Add(orderDetails);
      }
      else
      {
        orderDetails.quantity++;
      };

      var serializedJson = JsonConvert.SerializeObject(deserializedJson);

      HttpContext.Session.SetString(SESSION_CART, serializedJson);

      var product = productRepository.getProductById(id);
      return RedirectToAction("Index", "Product", new { message = $"Product {product.description} added successfully!" });
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

      var deserializedJson = deserializeSessionCart();
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


    [HttpPost]
    public IActionResult BuyAll(List<OrderDetails> orderDetails)//add [FromBody] to use javascript
    {

      var order = new Order
      {

        user_id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
        creationDate = DateTime.Now,
        orderDetails = orderDetails

      };

      orderRepository.placeOrder(order);

      foreach (var cookie in Request.Cookies.Keys)
      {
        if (cookie == ".ProductManagement.Session")
          Response.Cookies.Delete(cookie);
      }

      return Redirect("../Order/OrderHistory");
    }


    private List<OrderDetails> deserializeSessionCart()
    {
      var cartJson = HttpContext.Session.GetString(SESSION_CART);

      if (!String.IsNullOrEmpty(cartJson))
      {
        List<OrderDetails> deserializedJson = JsonConvert.DeserializeObject<List<OrderDetails>>(cartJson);
        return deserializedJson;
      }
      return new List<OrderDetails>();
    }



  }
}