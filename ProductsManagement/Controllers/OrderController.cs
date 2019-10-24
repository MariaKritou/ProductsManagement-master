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
      var orderVm = new OrderVM();
      orderVm.product = productRepository.getProductById(id);
      return View(orderVm);
    }

    [HttpPost]
    public IActionResult PostOrder(OrderVM orderVM)
    {
      orderVM.product = productRepository.getProductById(orderVM.product.id);

      if (ModelState.IsValid)
      {        
        var order = new Order
        {
          product_id = orderVM.product.id,
          user_id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
          creationDate = DateTime.Now,
          quantity = orderVM.quantity
        };

        orderRepository.placeOrder(order);
       
        return Redirect("../Product/Index");
      }

      return View("Order",orderVM);
    }


    public IActionResult orderHistory()
    {
      var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    
      return View(orderRepository.getOrderByUserId(id));
    }

    }
}