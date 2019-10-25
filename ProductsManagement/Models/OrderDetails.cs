using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Models
{
  public class OrderDetails
  {
    public int id { get; set; }
    public int orderId { get; set; }
    public int productId{ get; set; }
    public int quantity { get; set; }
  }
}
