using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsManagement.Models;
using ProductsManagement.Repositories;

namespace ProductsManagement.Controllers
{
  [Authorize]
  public class ProfileController : Controller
  {
    private readonly UserRepository userRepository;

    public ProfileController(UserRepository userRepository)
    {
      this.userRepository = userRepository;
    }

    public IActionResult Index()
    {
      //για να παρει τα δεδομενα το Index πρωτα τα περναω εδω 
      //ViewData["currentUser"] = User?.Identity?.Name; ? = in case this object doesnt exist, execute the next gets/return null
      //ViewData["currentClaim"] = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
      ViewData["Message"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
      return View(userRepository.getUserByUsername(User.FindFirst("username")?.Value));
    }

    public IActionResult Edit(int id)//ενω δεν εχει id το user το περναω ετσι γιατι το ζηταει 
    {                                  //το buttonpartialmodel ουσιαστικα ειναι το username
      return View(userRepository.getUserById(id));
    }

    [HttpPost]
    public IActionResult PostEdit(User user)
    {
      userRepository.editUser(user);
      return RedirectToAction("Index");
    }

  }
}