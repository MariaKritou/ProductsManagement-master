using ProductsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Repositories
{
  public class ProductMemoryRepository : IProductRepository
  {
    List<Product> products = new List<Product>();

    public ProductMemoryRepository()
    {
      Random rnd = new Random();

      for (int i = 0; i < 100; i++)
      {
        //var product = new Product(i, $"product {i}", rnd.Next(10, 50), rnd.Next(10, 50), DateTime.Now.AddDays(new Random().Next(1000)));
        var product = new Product
        {
          id = i,
          description = $"product {i}",
          price = rnd.Next(10, 50),
          quantity = rnd.Next(10, 50),
          arrivalDate = DateTime.Now.AddDays(new Random().Next(1000))
        };
        products.Add(product);
      }
    }

    public List<Product> getAllProducts(int? take=null, int? skip=null)
    {
      var result = products;
      if(take.HasValue)
      {
        result = result.Take(take.Value).ToList();
      }
      if (skip.HasValue)
      {
        result = result.Skip(skip.Value).ToList();
      }
      return result;
    }

    public Product getProductById(int id)
    {
      var product = products.FirstOrDefault(p => p.id == id);
      return product;
    }
    public void deleteProductById(int id)
    {
      var product = products.FirstOrDefault(p => p.id == id);
      products.Remove(product);

    }

    public void createProduct(Product product)
    {
      
        products.Add(product);      
    }

    public void editProduct(Product product)
    {
      var oldproduct = getProductById(product.id);
      oldproduct.description = product.description;
      oldproduct.price = product.price;
      oldproduct.quantity = product.quantity;
    }

    public List<Product> getAllProductsByDescription(string searchString)
    {
      var product = products.Where(p => p.description.Contains(searchString)).ToList();
      return product;
    }

  
  }
}
