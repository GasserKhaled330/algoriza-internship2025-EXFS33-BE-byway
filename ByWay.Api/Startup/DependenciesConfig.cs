using ByWay.Api.Mappers;
using ByWay.Application.Services;
using ByWay.Domain.Interfaces.Service;
using ByWay.Domain.Interfaces.UnitOfWork;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using ByWay.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ByWay.Api.Startup
{
  internal static class DependenciesConfig
  {
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
      builder.AddSerilog();

      builder.Services.AddCors(options =>
      {
        options.AddDefaultPolicy(
          policy =>
          {
            policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
          });
      });

      builder.Services.AddControllers().AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
      });

      builder.Services.AddDbContext<AppDbContext>(opt =>
      opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

      //builder.Services.AddSeeders();
      builder.AddJWT();
      builder.Services.AddSwaggerServices();
      builder.Services.AddHttpContextAccessor();

      builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
      builder.Services.AddScoped<IInstructorService, InstructorService>();
      builder.Services.AddScoped<ICategoryService, CategoryService>();
      builder.Services.AddScoped<ICourseService, CourseService>();
      builder.Services.AddScoped<IAuthService, AuthService>();
      builder.Services.AddScoped<ICartService, CartService>();
      builder.Services.AddScoped<ICheckoutService, CheckoutService>();
      builder.Services.AddScoped<IImageService, ImageService>();
      builder.Services.AddScoped<IEmailService, EmailService>();

      builder.Services.AddSingleton<InstructorMapper>();
      builder.Services.AddSingleton<CategoryMapper>();
      builder.Services.AddSingleton<CourseMapper>();

    }
  }
}
