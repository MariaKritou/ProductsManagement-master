using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProductsManagement.Models;
using ProductsManagement.Repositories;


namespace ProductsManagement.Controllers
{
  public class AccountController : Controller
  {
    private readonly UserRepository userRepository;

    public AccountController(UserRepository userRepository)
    {
      this.userRepository = userRepository;
    }

    [HttpGet]
    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
      if (ModelState.IsValid)
      {

        var user = userRepository.login(model.Username, model.Password);
        if (user != null)
        {
          var claims = new List<Claim>
        {
          new Claim(ClaimTypes.Role, "user"),
          new Claim("age", user.age.ToString()),
          new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
          new Claim(ClaimTypes.Name, user.firstName),
          new Claim("username", user.username)
          //claim me id
        };

          if (user.isAdmin)
          {
            claims.Add(new Claim(ClaimTypes.Role, "admin"));
          }

          //if (user.age > 18)
          //{
          //  claims.Add(new Claim(ClaimTypes.Role, "admin"));
          //}

          var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
          await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

          return RedirectToAction("Index", "Profile");
        }
        return View();
      }
      return View("Login");
    }

    //private bool LoginUser(string username, string password)
    //{
    //  return true;
    //}

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      return RedirectToAction("Index", "DataGrid");
    }

    [HttpGet]
    public IActionResult UnauthorizedView()
    {
      return View();
    }


    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public IActionResult PostCreate(User user)
    {
      userRepository.createUser(user);

      return RedirectToAction("Login");
    }


  }
}
