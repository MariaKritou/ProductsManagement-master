using SQW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Models
{
  public class Product
  {
    [SQWFieldMap("PRODUCT_ID")]
    //[Required(ErrorMessage = "The field {0} is required")]
    [Display(Name = "Id")]
    public int id { get; set; }

    [SQWFieldMap("DESCRIPTION")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "The field {0} is required")]
    [Display(Name = "Description")]
    public string description { get; set; }

    [SQWFieldMap("PRICE")]
    [Required(ErrorMessage = "The field {0} is required")]
    [Range(5, 500, ErrorMessage = "The range must be between {0} and {1}")]
    [Display(Name = "Price")]
    public float price { get; set; }

    [SQWFieldMap("QUANTITY")]
    [DisplayName("Quantity")]
    public int quantity { get; set; }

    [SQWFieldMap("ARRIVAL_DATE")]
    [DisplayName("Date")]
    public DateTime arrivalDate { get; set; }

    public string descriptionUpper => !string.IsNullOrEmpty(description) ? description[0].ToString().ToUpper() + description.Substring(1) : string.Empty;//productcardpartial   
    
    public Product()
    {
    }

    public Product(int id, string description, float price, int quantity, DateTime arrivalDate)
    {
      this.id = id;
      this.description = description;
      this.price = price;
      this.quantity = quantity;
      this.arrivalDate = arrivalDate;
    }
  }
}
