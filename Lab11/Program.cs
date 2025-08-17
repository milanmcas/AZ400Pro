using Microsoft.FeatureManagement;

namespace Lab11
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //const string ConnectionString = "Endpoint=https://appcs-53852658.azconfig.io;Id=pFK4;Secret=5JjBVCVF96ZTpmmkKewPL0LF0EBmsTMv3suvoWlRTVQ1wOgQ3ubuJQQJ99BHAC8vTIndyv2KAAACAZAC8YWL";

            builder.Services.Configure<MyConfig>(builder.Configuration.GetSection("MyNiceConfig"));
            //builder.Configuration.AddAzureAppConfiguration(options =>
            //{
            //    options.Connect(ConnectionString).ConfigureRefresh((refreshOptions) =>
            //    {
            //        // indicates that all configuration should be refreshed when the given key has changed.
            //        refreshOptions.Register(key: "MyNiceConfig:PageSize", refreshAll: true).SetCacheExpiration(TimeSpan.FromSeconds(10));
            //        //refreshOptions.SetCacheExpiration(TimeSpan.FromSeconds(5));
            //    }).UseFeatureFlags();
            //});
            //builder.Configuration.AddAzureAppConfiguration(options =>
            //{
            //    options.Connect(ConnectionString)
            //           .ConfigureRefresh(refresh =>
            //           {
            //               refresh.Register("MyNiceConfig:PageSize", true);
            //           })
            //           .Select("MyNiceConfig:*");
            //});
            builder.Services.AddAzureAppConfiguration();
            builder.Services.AddFeatureManagement();
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAzureAppConfiguration();

            app.MapControllers();

            app.Run();
        }
    }
}
