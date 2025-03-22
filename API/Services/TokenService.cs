using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
  public string CreateToken(AppUser user)
  {
    var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings");
    if (tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer");

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

    var claims = new List<Claim>
    {
      new(ClaimTypes.NameIdentifier, user.UserName)
    };

    // var key = Encoding.UTF8.GetBytes(tokenKey);
    // but it won’t work correctly when signing the JWT. Here's why:

    // 🔴 Why Can't We Directly Use byte[] key = Encoding.UTF8.GetBytes(tokenKey);?
    // The byte[] alone is not enough for .NET's JWT signing mechanism. You must wrap it in SymmetricSecurityKey because:

    // 1️⃣ SigningCredentials Requires a SecurityKey
    // SigningCredentials does not accept a byte[] directly.
    // It specifically needs a SecurityKey, which is an object that tells .NET how to use the key.
    // If you try:

    // csharp
    // Copy
    // Edit
    // var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
    // ❌ This will throw an error because key is just a byte[], not a SecurityKey

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = creds
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);

    //     we are not writing the token to a file or database. 
    // Instead, we are converting the token into a string representation and returning it.

    // 🔹 What Happens in WriteToken(token)?
    // WriteToken(token) takes the JWT token object (SecurityToken).
    // It serializes it into a string in JWT format (Base64-encoded string).
    // The returned string can be sent in HTTP responses (like in an API).

    //     example
    //     HEADER.PAYLOAD.SIGNATURE

    // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9
    // .
    // eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiZXhwIjoxNzA5MTk3NzYwfQ
    // .
    // jlh64T4AGm9ZRgEX4PlC7HYNOOJK...

  }
}
