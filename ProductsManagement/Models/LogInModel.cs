using SQW;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Models
{
  public class LoginModel
  {
    
    [Display(Name = "Username")]
    [Required(ErrorMessage = "The field {0} is required")]
    public string Username { get; set; }


    [Display(Name = "Password")]
    [Required(ErrorMessage = "The field {0} is required")]
    public string Password { get; set; }
  }
}
