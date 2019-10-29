using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsManagement.Models;
using ProductsManagement.Repositories;

namespace ProductsManagement.Controllers
{
  public class OrderController : Controller
  {

    private readonly OrderRepository orderRepository;
    private readonly IProductRepository productRepository;

    public OrderController(OrderRepository orderRepository, IProductRepository productRepository)
    {
      this.orderRepository = orderRepository;
      this.productRepository = productRepository;
    }

    public IActionResult Order(int id)
    {
      var orderDetails = new OrderDetails();
      orderDetails.product = productRepository.getProductById(id);
      orderDetails.productId = id;
      return View(orderDetails);
    }

    [HttpPost]
    public IActionResult PostOrder(OrderDetails orderDetails)
    {

      if (ModelState.IsValid)
      {
        var order = new Order
        {

          user_id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
          creationDate = DateTime.Now,
          orderDetails = new List<OrderDetails>
          {
           orderDetails
          }

        };

        orderRepository.placeOrder(order);

        return Redirect("../Product/Index");
      }

      return View("Order", orderDetails);
    }


    public IActionResult OrderHistory()
    {
      var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

      var orders = orderRepository.getOrderByUserId(id);

      foreach (var order in orders)
      {
        order.orderDetails = orderRepository.getOrderDetailsByOrderId(order.id);

        //foreach (var orderDetail in order.orderDetails)
        //{
        //  orderDetail.product = productRepository.getProductById(orderDetail.productId);
        //}
      }

      return View(orders);
    }

  }
}