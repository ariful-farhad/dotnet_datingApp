using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{


  [HttpPost("register")]
  // right now our controller cannot tell where to look for username and password in the request, so we have to give it hints
  // defaul behaviour is seeking from query parameters

  // public async Task<ActionResult<AppUser>> Register([FromBody] string username, string password)
  public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
  {
    if (await UserExists(registerDto.Username))
      return BadRequest("Username is taken");
    return Ok();

    // // if we don't pass a value as parameter, each time it will create a random key for hasing, 
    // // we need to store that key for veficiation
    // using var hmac = new HMACSHA512();
    // var user = new AppUser
    // {
    //   // as computehash takes bytearray
    //   //       Verification (During Login)
    //   // When a user logs in:

    //   // Retrieve the stored salt from the database.
    //   // Recompute the hash using the input password and the stored salt:

    //   // using var hmac = new HMACSHA512(storedSalt);
    //   // byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));

    //   UserName = registerDto.Username.ToLower(),
    //   PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
    //   PasswordSalt = hmac.Key
    // };

    // context.Users.Add(user);
    // await context.SaveChangesAsync();
    // return new UserDto
    // {
    //   Username = user.UserName,
    //   Token = tokenService.CreateToken(user)
    // };
  }

  [HttpPost("login")]
  public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
  {

    var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

    if (user == null)
    {
      return Unauthorized("Invalid username");
    }

    using var hmac = new HMACSHA512(user.PasswordSalt);

    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

    for (int i = 0; i < computedHash.Length; i++)
    {
      if (user.PasswordHash[i] != computedHash[i])
        return Unauthorized("password didn't match");
    }

    return new UserDto
    {
      Username = user.UserName,
      Token = tokenService.CreateToken(user)
    };




  }



  private async Task<bool> UserExists(string username)
  {
    return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
  }
}
