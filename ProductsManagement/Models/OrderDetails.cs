using SQW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Models
{
  public class OrderDetails
  {
    [SQWFieldMap("ID")]
    public int id { get; set; }

   
    public Order order { get; set; }
    [SQWFieldMap("ORDER_ID")]
    public int orderId { get; set; }
    [SQWFieldMap("CREATED_AT")]
    public DateTime orderCreatedAt { get; set; }

    public Product product { get; set; }
    [SQWFieldMap("PRODUCT_ID")]
    public int productId{ get; set; }
    [SQWFieldMap("DESCRIPTION")]
    public string productDescription { get; set; }
    [SQWFieldMap("PRICE")]
    public int productPrice { get; set; }

    [SQWFieldMap("QUANTITY")]
    public int quantity { get; set; }
   

  }
}
