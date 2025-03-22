// after copy and paste
// 
// 
// 

// using System;

// namespace API.Extentions;

// public static class IdentityServiceExtensions
// {
//   public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
//   {
//     builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//     {
//       var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Token key not found");
//       options.TokenValidationParameters = new TokenValidationParameters
//       {
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
//         ValidateIssuer = false,
//         ValidateAudience = false,
//       };

//     });
//     return services;
//   }
// }



// after modification
// 
// 
// 
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extentions;

public static class IdentityServiceExtensions
{
  public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
      var tokenKey = config["TokenKey"] ?? throw new Exception("Token key not found");
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
      };

    });
    return services;
  }
}
