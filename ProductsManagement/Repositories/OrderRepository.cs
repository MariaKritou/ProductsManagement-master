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
          var orderId = context.createSpCommand("MARIADEMO.MAIN.PLACE_ORDER")
                        .addNumericInParam("A_USER_ID", order.user_id)
                        .addDateTimeInParam("A_CREATED_AT", order.creationDate)
                        .addNumericOutParam("A_ORDER_ID")
                        .execute()
                        .paramValue<int>("A_ORDER_ID");

          foreach (var orderDetail  in order.orderDetails)
          {
            context.createSpCommand("MARIADEMO.MAIN.PLACE_ORDER_DETAILS")
                  .addNumericInParam("A_ORDER_ID", orderId)
                  .addNumericInParam("A_PRODUCT_ID", orderDetail.productId)
                  .addNumericInParam("A_QUANTITY", orderDetail.quantity)
                  .execute();
          }
    


        }
        catch (Exception ex)
        {

          Console.WriteLine(ex.Message);
        }
      
      });
    }

    //public void placeOrderDetails(int productId, int orderId, int quantity)
    //{
    //  worker.run(context =>
    //  {
    //    context.createSpCommand("MARIADEMO.MAIN.PLACE_ORDER_DETAILS")
    //     .addNumericInParam("A_ORDER_ID", orderId)
    //     .addNumericInParam("A_PRODUCT_ID", productId)
    //     .addNumericInParam("A_QUANTITY", quantity)
    //     .execute();
    //  });
    //}


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
