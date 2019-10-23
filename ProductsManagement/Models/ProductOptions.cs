using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Models
{
  public class ProductOptions
  {
    public bool showCards { get; set; } = true;
    public int maxProductsCount { get; set; } = 20;

  }
}
