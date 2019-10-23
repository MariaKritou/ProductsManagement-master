using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Models
{
  public class CartVM
  {
    [Display(Name = "Id")]
    public int productId { get; set; }

    [Display(Name = "Description")]
    public string description { get; set; }

    [Display(Name = "Price")]
    public float price { get; set; }

    [Display(Name = "Quantity")]
    public int quantity { get; set; }
  
  }
}
