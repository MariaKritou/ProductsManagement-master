using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductsManagement.Models;
using SQW.Interfaces;

namespace ProductsManagement.Repositories
{
  public class ProductDatabaseRepository : IProductRepository
  {
    private readonly ISQWWorker worker;

    public ProductDatabaseRepository(ISQWWorker worker)
    {
      this.worker = worker;
    }

    public void createProduct(Product product)
    {
      worker.run(context =>
      {
        context
          .createSpCommand("MARIADEMO.MAIN.CREATE_PRODUCT")
          .addStringInParam("A_DESCRIPTION", product.description)
          .addNumericInParam("A_PRICE", product.price)
          .addNumericInParam("A_QUANTITY", product.quantity)
          .addDateTimeInParam("A_ARRIVAL_DATE", product.arrivalDate)
          .execute();
      });
    }

    public void deleteProductById(int id)
    {
      worker.run(context =>
      {
        //context
        //  .createCommand("DELETE FROM MARIADEMO.PRODUCTS WHERE PRODUCT_ID = :ID")
        //  .addNumericInParam("ID", id)
        //  .execute();

        context
        .createSpCommand("MARIADEMO.MAIN.DELETE_PRODUCT_BY_ID")
        .addNumericInParam("A_PRODUCT_ID", id)
        .execute();
      });
    }

    public void editProduct(Product product)
    {
      try
      {
        worker.run(context =>
        {
          context
            .createSpCommand("MARIADEMO.MAIN.EDIT_PRODUCT")
            .addStringInParam("A_DESCRIPTION", product.description)
            .addNumericInParam("A_PRICE", product.price)
            .addNumericInParam("A_QUANTITY", product.quantity)
            .addDateTimeInParam("A_ARRIVAL_DATE", product.arrivalDate)
            .addNumericInParam("A_PRODUCT_ID", product.id)
            .execute();
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    public List<Product> getAllProducts(int? take = null, int? skip = null)
    {
      List<Product> products = null;

      worker.run(context =>
      {
        products = context
          .createSpCommand("MARIADEMO.MAIN.GET_ALL_PRODUCTS")
          .addCursorOutParam("RET_VAL")
          .select<Product>();
      });

      return products;
    }

    public List<Product> getAllProductsByDescription(string searchString)
    {
      List<Product> product = null;

      worker.run(context =>
      {
        product = context
          .createSpCommand("MARIADEMO.MAIN.GET_ALL_PRODUCTS_BY_DESCRIPTION")
          .addCursorOutParam("RET_VAL")
          .addStringInParam("A_DESCRIPTION", $"%{searchString}%")
          .select<Product>();
      });

      return product;
    }

    public Product getProductById(int id)
    {
      Product product = null;

      worker.run(context =>
      {
        product = context
          .createSpCommand("MARIADEMO.MAIN.GET_PRODUCT_BY_ID")
          .addCursorOutParam("RET_VAL")
          .addNumericInParam("A_PRODUCT_ID", id)
          .first<Product>();
      });

      return product;
    }
  }
}
