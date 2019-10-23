using ProductsManagement.Models;
using SQW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Repositories
{
  public class OrderRepository
  {
    private readonly ISQWWorker worker;

    public OrderRepository(ISQWWorker worker)
    {
      this.worker = worker;
    }

    public void placeOrder(Order order)
    {
      worker.run(context =>
      {
        //context.createCommand("INSERT INTO MARIADEMO.ORDERS (PRODUCT_ID, USER_ID, CREATED_AT, QUANTITY) VALUES (:PRODUCT_ID, :USER_ID, :CREATED_AT, :QUANTITY)")
        //.addNumericInParam("PRODUCT_ID", order.product_id)
        //.addNumericInParam("USER_ID", order.user_id)
        //.addDateTimeInParam("CREATED_AT", order.creationDate)
        //.addNumericInParam("QUANTITY", order.quantity)
        //.execute();
        try
        {
          context.createSpCommand("MARIADEMO.MAIN.PLACE_ORDER")
          .addNumericInParam("A_PRODUCT_ID", order.product_id)
          .addNumericInParam("USER_ID", order.user_id)
          .addDateTimeInParam("CREATED_AT", order.creationDate)
          .addNumericInParam("A_QUANTITY", order.quantity)
          .execute();
        }
        catch (Exception ex)
        {

          Console.WriteLine(ex.Message);
        }
      
      });
    }


    public List<Order> getOrderByUserId(int id)
    {
      List<Order> order = null;

      worker.run(context =>
      {
         order = context
        .createSpCommand("MARIADEMO.MAIN.GET_ORDER_BY_USER_ID")
        .addCursorOutParam("RET_VAL")
        .addNumericInParam("A_USER_ID", id)
        .select<Order>();
      });

      return order;
    }
  }
}
