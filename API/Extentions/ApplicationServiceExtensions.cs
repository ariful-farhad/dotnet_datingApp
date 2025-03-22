using System;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
  {
    // Add services to the container.

    services.AddControllers();
    // it will create an instance of DataContext class and then pass the object specified in the parameter
    services.AddDbContext<DataContext>(opt =>
    {
      opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
    });

    //adding cors
    services.AddCors();
    // for each client, a tokenservice will be created
    services.AddScoped<ITokenService, TokenService>();

    return services;
  }

}
