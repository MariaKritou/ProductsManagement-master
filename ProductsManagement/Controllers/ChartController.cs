using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsManagement.Repositories;

namespace ProductsManagement.Controllers
{
    public class ChartController : Controller
  {
        private readonly IProductRepository productRepository;

        public ChartController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IActionResult QuantityPerProduct()
        {
            return View(productRepository.getAllProducts(skip:80));
        }
    }
}