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
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    /*
    // AppDomain.CurrentDomain.GetAssemblies(): This is the crucial part that tells AddAutoMapper where to look for your mapping definitions (profiles).

    AppDomain.CurrentDomain: Represents the current running environment of your application.
    GetAssemblies(): Gets a list of all the compiled code assemblies (like your API.dll and any referenced library DLLs) that are currently loaded into your application's memory.
    Putting it together:

    This single line tells the ASP.NET Core application:

    "Register the AutoMapper service (IMapper) so I can inject and use it later. To configure it, automatically scan all the assemblies currently loaded in my application, find any classes that inherit from AutoMapper.Profile (like your API/Helpers/AutoMapperProfiles.cs), and load the mapping rules defined inside them."
  */
    return services;
  }

}
