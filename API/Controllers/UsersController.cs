using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
// [ApiController]
// [Route("api/[Controller]")]
// public class UsersController : ControllerBase
// {
//     private readonly DataContext context;

//     public UsersController(DataContext context)
//   {
//         this.context = context;
//    }

// }


public class UsersController(DataContext context) : BaseApiController
{
  // [HttpGet]
  // public ActionResult<IEnumerable<AppUser>> GetUsers()
  // {
  //   var users = context.Users.ToList();
  //   return Ok(users);
  // }
  [AllowAnonymous]
  [HttpGet]
  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
  {
    var users = await context.Users.ToListAsync();
    return users;
  }
  [Authorize]
  [HttpGet("{id:int}")] // /api/users/2
  public async Task<ActionResult<AppUser>> GetUser(int id)
  {
    var user = await context.Users.FindAsync(id);
    if (user == null) return NotFound();
    return user;
  }


}
