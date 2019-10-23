using SQW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Models
{
  public class User
  {
    [SQWFieldMap("USER_ID")]
    public int id { get; set; }

    [SQWFieldMap("USERNAME")]
    [Required(ErrorMessage = "The field {0} is required")]
    [Display(Name="Username")]
    public string username { get; set; }

    [SQWFieldMap("PASSWORD")]
    [Required(ErrorMessage = "The field {0} is required")]
    [Display(Name="Password")]
    public string password { get; set; }

    [SQWFieldMap("FIRSTNAME")]
    [Required(ErrorMessage ="The filed {0} is required")]
    [Display(Name="First Name")]
    public string firstName { get; set; }

    [SQWFieldMap("LASTNAME")]
    [Required(ErrorMessage = "The filed {0} is required")]
    [Display(Name="Last Name")]
    public string lastName { get; set; }

    [SQWFieldMap("AGE")]
    [Required(ErrorMessage = "The filed {0} is required")]
    [Display(Name="Age")]
    public int age { get; set; }

    [SQWFieldMap("ISADMIN")]
    public bool isAdmin { get; set; }

    public User()
    {

    }
    public User(string username, string firstName, string lastName, int age, string password, bool isAdmin)
    {
      this.username = username;
      this.firstName = firstName;
      this.lastName = lastName;
      this.password = password;
      this.isAdmin = isAdmin;
      this.age = age;
    }
  }


}
