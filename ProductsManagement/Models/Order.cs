﻿using SQW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Models
{
  public class Order
  {
    [SQWFieldMap("ORDER_ID")]
    public int id { get; set; }

    [SQWFieldMap("USER_ID")]
    public int user_id { get; set; }

    [SQWFieldMap("CREATED_AT")]
    public DateTime creationDate { get; set; }

    public List<OrderDetails> orderDetails { get; set; }


  }
}
