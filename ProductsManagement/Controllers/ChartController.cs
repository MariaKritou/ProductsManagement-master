using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsManagement.Repositories;

namespace ProductsManagement.Controllers
{
  public class ChartController : Controller
  {
    private readonly IProductRepository productRepository;
    private readonly OrderRepository orderRepository;

    public ChartController(IProductRepository productRepository, OrderRepository orderRepository)
    {
      this.productRepository = productRepository;
      this.orderRepository = orderRepository;
    }

    public IActionResult QuantityPerProduct()
    {
      return View(productRepository.getAllProducts(skip: 80));
    }

    public IActionResult ExpensesHistoryChart()
    {
      var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

      var orders = orderRepository.getOrderDetailsByUserId(id);

      return View(orders);
    }
  }
}