using API.Data;
using API.Extentions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// ######################################## Services #########################################
// these are moved to application service extenstion class

// // Add services to the container.

// builder.Services.AddControllers();
// // it will create an instance of DataContext class and then pass the object specified in the parameter
// builder.Services.AddDbContext<DataContext>(opt =>
// {
//     opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
// });


// //adding cors
// builder.Services.AddCors();
// // for each client, a tokenservice will be created
// builder.Services.AddScoped<ITokenService, TokenService>();




// after moving the above services, we can replace it like this
builder.Services.AddApplicationServices(builder.Configuration);

// for validating jwt tokens
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
// {
//     var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Token key not found");
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
//         ValidateIssuer = false,
//         ValidateAudience = false,
//     };

// });



// alternative of the above code
builder.Services.AddIdentityServices(builder.Configuration);




// ######################################## MIDDLEWARE #########################################


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{

    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migrations");
}

app.Run();
