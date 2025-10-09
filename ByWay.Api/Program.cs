using ByWay.Api.Startup;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Remove the server header
builder.WebHost.ConfigureKestrel(opt => opt.AddServerHeader = false);
// Add services to the container.
builder.AddDependencies();

var app = builder.Build();

//await app.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.

app.UseOpenApiSwagger();

app.UseSerilogRequestLogging();

app.UseException();

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{

  OnPrepareResponse = ctx =>
  {
    ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=604800");
  }
});

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
