using ProductsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Repositories
{
    public interface IProductRepository
    {
        List<Product> getAllProducts(int? take = null, int? skip = null);
        Product getProductById(int id);
        void deleteProductById(int id);
        void createProduct(Product product);
        void editProduct(Product product);
        List<Product> getAllProductsByDescription(string searchString);
    }
}
