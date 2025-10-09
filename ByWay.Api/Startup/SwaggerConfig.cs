using Microsoft.OpenApi.Models;

namespace ByWay.Api.Startup
{
  internal static class SwaggerConfig
  {
    internal static void AddSwaggerServices(this IServiceCollection services)
    {
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen(opt =>
      {
        opt.SwaggerDoc("v1", new OpenApiInfo() { Title = "ByWay API", Version = "v1" });
        opt.EnableAnnotations();

        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Description = """
                        JWT Authorization header using the Bearer scheme.
                                              Enter 'Bearer' [space] and then your token in the text input below.
                                              Example: 'Bearer 12345abcdef'
                        """,
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.Http,
          Scheme = "Bearer",
          BearerFormat = "JWT"
        });

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
            },
            new List<string>()
          }
        });
      });
    }

    internal static void UseOpenApiSwagger(this WebApplication app)
    {
      //if (app.Environment.IsDevelopment())
      //{
      app.UseSwagger();
      app.UseSwaggerUI();
      //}
    }
  }
}
