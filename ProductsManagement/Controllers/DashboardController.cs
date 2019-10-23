using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsManagement.Models;
using ProductsManagement.Repositories;

namespace ProductsManagement.Controllers
{
  // [Authorize(Policy= "admin")]
    public class DashboardController : Controller
  {
        private readonly IProductRepository productRepository;

        public DashboardController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IActionResult Index()
        { 
           return View(new Dashboard {product = productRepository.getAllProducts(skip:90)});
        }

    }
}