using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Models
{
  public class OrderVM : IValidatableObject
  {
    public Product product { get; set; }

    [Required]
    public int quantity { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (quantity > product.quantity)
      {
        yield return new ValidationResult(
          $"Quantity must be less or equal {product.quantity}.", 
          new[] {"quantity"});
      }
    }
  }
}
