using System.Text;
using ByWay.Api.Helper;
using ByWay.Domain.Entities;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ByWay.Api.Startup;

public static class JWTAuthConfig
{
    public static void AddJWT(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
      
        builder
            .Services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
      
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
        });
        builder
            .Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])
                    )
                };
            });
    }
}