using Serilog;
namespace ByWay.Api.Startup
{
  internal static class SerilogConfig
  {
    public static void AddSerilog(this WebApplicationBuilder builder)
    {
      builder.Host.UseSerilog((context, services, configuration) =>
        configuration
          .ReadFrom.Configuration(context.Configuration)
          .ReadFrom.Services(services)
          .Enrich.FromLogContext()
      );
    }

  }
}
