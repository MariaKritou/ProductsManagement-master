using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProductsManagement.Repositories;

namespace ProductsManagement.Controllers
{
  [Authorize]
  public class ChatController :Controller
  {
    private readonly UserRepository userRepository;

    public ChatController(UserRepository userRepository)
    {
      this.userRepository = userRepository;
    }

    public IActionResult Chat()
    {

      ViewData["Message"] = "Chat page.";
      ViewData["baseUrl"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
      return View(userRepository.getUserByUsername(User.FindFirst("username")?.Value));

    }
  }
}